using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Castle.Core.Logging;
using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Services
{
    public class UserAvatarService : IUserAvatarService
    {
        public const int AvatarMaxHeight = 200;
        public const int AvatarMaxWidth = 200;
        public const int ImageQuality = 70;

        public ILogger Logger { get; set; }
        private readonly IUserRepository _userRepository;
        
        public UserAvatarService(IUserRepository userRepository)
        {
            _userRepository = userRepository;                        
        }

        //returns Point where x is start on original line segment and y is end
        private static Point CalcCenterCropCoordinates(int srclength, int maxlength)
        {
            Point crop = new Point();
            if (srclength > maxlength)
            {
                crop.X = (srclength - maxlength) / 2;
                crop.Y = crop.X + maxlength;
            }
            else
            {
                crop.X = 0;
                crop.Y = srclength;
            }
            return crop;
        }

        private static Rectangle CalcCropCoordinates(int srcHeight, int srcWidth, int maxHeight, int maxWidth)
        {
            Rectangle crop = new Rectangle();
            //calculate crop positions for height and width
            Point widthcrop = CalcCenterCropCoordinates(srcWidth, maxWidth);
            Point heightcrop = CalcCenterCropCoordinates(srcHeight, maxHeight);
            //apply to rectangle
            crop.Location = new Point(widthcrop.X, heightcrop.X);
            crop.Height = heightcrop.Y - heightcrop.X;
            crop.Width = widthcrop.Y - widthcrop.X;
            return crop;
        }

        private Image CropToFit(Image srcImg, int maxHeight, int maxWidth)
        {
            Rectangle cropCoords = CalcCropCoordinates(srcImg.Height, srcImg.Width, maxHeight, maxWidth);
            Bitmap result = new Bitmap(cropCoords.Width, cropCoords.Height, PixelFormat.Format24bppRgb);
            using (Graphics dst = Graphics.FromImage(result))
            {
                dst.DrawImage(srcImg, 0, 0, cropCoords, GraphicsUnit.Pixel);
            }
            return result;
        }

        private Size ResizeForCrop(int originalHeight, int originalWidth, int maxHeight, int maxWidth)
        {
            Size newSize = new Size(0, 0);

            //how much resizing do we need
            float scaleFactorHeight = ((float)maxHeight / (float)originalHeight);
            float scaleFactorWidth = ((float)maxWidth / (float)originalWidth);

            //pick the least drastic (so the smallest dimension fits our bounding box)
            float scaleFactor = Math.Max(scaleFactorWidth, scaleFactorHeight);

            //calculate our new dimensions
            newSize.Width = Math.Max((int)(scaleFactor * originalWidth), maxWidth);
            newSize.Height = Math.Max((int)(scaleFactor * originalHeight), maxHeight);
            return newSize;
        }

        private Image ResizeAndCropToFill(Image srcImg, int width, int height)
        {
            Image result = new Bitmap(srcImg, ResizeForCrop(srcImg.Height, srcImg.Width, height, width));
            return CropToFit(result, height, width);
        }

        private void SaveAsCompressedJpg(Image img, string path)
        {
            //find our png ici
            ImageCodecInfo iciJpg = null;
            foreach (ImageCodecInfo ici in ImageCodecInfo.GetImageDecoders())
            {
                if (ici.FilenameExtension.ToLower().Contains("jpg"))
                {
                    iciJpg = ici;
                    break;
                }
            }

            if (iciJpg == null)
            {
                throw new Exception("no jpg codec found on server");
            }
            //setup compression
            EncoderParameters eps = new EncoderParameters(1);
            EncoderParameter ep = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 75L);


            eps.Param[0] = ep;
            img.Save(path, iciJpg, eps);

        }

        private bool ResizeConvertAndSaveImage(Stream srcfile, string destinationPath)
        {
            try
            {
                //create a image from the stream
                using (Image image = Image.FromStream(srcfile))
                {
                    //create new image of the right size
                    using (Image avatarimage = ResizeAndCropToFill(image, AvatarMaxWidth, AvatarMaxHeight))
                    {
                        //convert to png
                        SaveAsCompressedJpg(avatarimage, destinationPath);
                        //avatarimage.Save(destinationPath, ImageFormat.Png);
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                //some uploaded rubbish or we are out of room.
                Logger.InfoFormat(e, "Avatar resize and save failed: {0}", e.Message);
                //eat the exception since it was probably the users fault.
                return false;
            }
        }

        private static string AddCacheBusterParam(string url)
        {
            return String.Format("{0}?cb={1:yyyyMMddhhmmss}", url, DateTime.UtcNow);
        }

        public void RemoveAvatar(User user, string avatarfolder)
        {
            
            //delete the file
            var fullpath = avatarfolder + "\\" + user.ProfileImageUrl;
            if (File.Exists(fullpath))
            {
                File.Delete(fullpath);
            }
            //clear the path
            user.ProfileImageUrl = null;
            _userRepository.Save(user);

        }

        public bool SetUserAvatarFromFile(User user, Stream srcfile, string fulldestpath, string urlbase)
        {
            string destfile = user.Id.ToString() + ".jpg";
            string destination = fulldestpath + destfile;
            string desturl = urlbase + destfile;
            //resize and save
            if (!ResizeConvertAndSaveImage(srcfile, destination)) return false;
            //success? update and commit
            user.ProfileImageUrl = AddCacheBusterParam(desturl);

            //save our changes since the file already exists on the filesystem.
            _userRepository.Save(user);

            return true;
        }

        public bool SetUserAvatarFromFile(User user, string srcfile, string fulldestpath, string urlbase)
        {
            //wrap the file in a stream
            using (var srcstream = new FileStream(srcfile, FileMode.Open))
            {
                return SetUserAvatarFromFile(user, srcstream, fulldestpath, urlbase);
            }
        }
    }
}
