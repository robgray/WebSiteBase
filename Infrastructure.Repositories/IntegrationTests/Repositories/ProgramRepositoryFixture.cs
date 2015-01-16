using System.Linq;
using FizzioFit.Common;
using FizzioFit.Domain.Entities;
using FizzioFit.Domain.Interfaces;
using FizzioFit.Domain.Services;
using FizzioFit.Infrastructure.Repositories.NHibernate;
using NHibernate;
using NUnit.Framework;

namespace IntegrationTests.Repositories
{
    [TestFixture]
    public class ProgramRepositoryFixture : ContainerFixture
    {
        private IProgramRepository _programRepository;
        private IEquipmentRepository _equipmentRepository;        
        private IClientRepository _clientRepository;
        private IUserRepository _userRepository;
        private IConditionRepository _conditionRepository;
        private IExercisePhaseRepository _exercisePhaseRepository;
        private ISession _session;
        private ICryptographyProvider _cryptographyProvider;
        
        [SetUp]
        public void SetUp()
        {
            _session = Container.Resolve<ISession>();
            var sessionManager = new SessionManager(() => _session);
            _cryptographyProvider = new CryptographyProvider();

            _programRepository = new ProgramRepository(sessionManager);
            _equipmentRepository = new EquipmentRepository(sessionManager);            
            _clientRepository = new ClientRepository(sessionManager);
            _userRepository = new UserRepository(sessionManager, _cryptographyProvider);
            _conditionRepository = new ConditionRepository(sessionManager);
            _exercisePhaseRepository = new ExercisePhaseRepository(sessionManager);
            
        }

        [Test]    
        public void Can_save_new_program_with_equipment()
        {            
            var equipment1 = _equipmentRepository.GetById(1);
            var equipment2 = _equipmentRepository.GetById(2);
            var equipment3 = _equipmentRepository.GetById(3);
            var client = _userRepository.GetById(2) as Client;
            var prescriber = _userRepository.GetById(1) as Prescriber;
            var condition = _conditionRepository.GetById(1);

            var program = new Program
                {
                    Name = "New Program",
                    Client = client,
                    Prescriber = prescriber,
                    ConditionTreated = condition,
                    CreatedDate = DateTimeHelper.Now
                };

            program.AddEquipment(equipment1);
            program.AddEquipment(equipment2);
            program.AddEquipment(equipment3);

            _programRepository.Save(program);
            _session.Flush();

            var pr = new ProgramRepository(new SessionManager(() => Container.Resolve<ISession>()));
            var loadedProgram = pr.GetById(program.Id);

            Assert.AreEqual(3, loadedProgram.ClientEquipment.Count());
        }

        [Test]
        public void Can_save_new_phase_to_new_program()
        {
            var ep = _exercisePhaseRepository.GetById(1);
            var phase = new Phase()
                {
					Duration = new Duration(5, DurationUnit.Days),
                    OrderInProgram = 1
                };
            var client = _userRepository.GetById(2) as Client;
            var prescriber = _userRepository.GetById(1) as Prescriber;
            var condition = _conditionRepository.GetById(1);

            var program = new Program
                {
                    Name = "Test Program",
                    Client = client,
                    Prescriber = prescriber,
                    ConditionTreated = condition,
                    CreatedDate = DateTimeHelper.Now
                };

            program.AddPhase(phase);
            _programRepository.Save(program);
            _session.Flush();
             
            var pr = new ProgramRepository(new SessionManager(() => Container.Resolve<ISession>()));
            var loadedProgram = pr.GetById(program.Id);

             Assert.AreEqual(1, loadedProgram.Phases.Count());
             Assert.IsFalse(phase.Id == 0);
        }

        [Test]
        public void Can_save_new_phase_to_existing_program()
        {
            var ep = _exercisePhaseRepository.GetById(1);
            var phase = new Phase()
            {
				Duration = new Duration(5, DurationUnit.Days),
                OrderInProgram = 1
            };

            var program = _programRepository.GetById(1);

            var phases = program.Phases.Count();

            program.AddPhase(phase);
            _programRepository.Save(program);
            _session.Flush();
            
            var pr = new ProgramRepository(new SessionManager(() => Container.Resolve<ISession>()));
            var loadedProgram = pr.GetById(program.Id);

            Assert.AreEqual(phases + 1, loadedProgram.Phases.Count());
            Assert.IsFalse(phase.Id == 0);
        }

        [Test]
        public void Can_delete_phase_from_existing_program()
        {
            // First Add phase.
            var ep = _exercisePhaseRepository.GetById(1);
            var phase = new Phase()
            {
				Duration = new Duration(5, DurationUnit.Days),
                OrderInProgram = 1
            };

            var program = _programRepository.GetById(1);

            program.AddPhase(phase);
            _programRepository.Save(program);
            _session.Flush();
                        
            var loadedProgram = _programRepository.GetById(program.Id);

            Assert.AreEqual(1, loadedProgram.Phases.Count());

            var existingPhase = loadedProgram.Phases.First();

            loadedProgram.RemovePhase(existingPhase);

            _programRepository.Save(loadedProgram);

            var pr2 = new ProgramRepository(new SessionManager(() => Container.Resolve<ISession>()));
            var loadedProgram2 = _programRepository.GetById(program.Id);

            Assert.AreEqual(0, loadedProgram.Phases.Count());
        }
        
       
        [TearDown]
        public void TearDown()
        {            
            Container.Release(_session);
        }
    }
}
