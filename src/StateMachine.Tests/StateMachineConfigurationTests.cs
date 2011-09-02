using NUnit.Framework;

namespace StateMachine.Tests
{
    [TestFixture]
    public class StateMachineConfigurationTests
    {
        [Test]
        public void api_concept()
        {
            
        }


        private class Vehicle
        {
            private bool _seatbeltOn;

            public Vehicle()
            {
                State = StateMachine<VehicleStates>.For<Vehicle>.Create(cfg =>
                {
                    cfg.Initial = VehicleStates.Parked;
                    cfg.BeforeTransition
                        .From(VehicleStates.Parked).To(States<VehicleStates>.Any - VehicleStates.Parked)
                        .Do(PutOnSeatBelt);

                    cfg.AfterTransition.On("Crash").Do(Tow);
                    cfg.AfterTransition.On("Repair").Do(Fix);
                    cfg.AfterTransition
                        .From(States<VehicleStates>.Any).To(VehicleStates.Parked)
                        .Do(vehicle => vehicle._seatbeltOn = false);
                });
            }

            private void PutOnSeatBelt()
            {
                _seatbeltOn = true;
            }

            private void Tow()
            {

            }

            private void Fix()
            {

            }

            public StateMachine<VehicleStates> State { get; private set; }
        }

        private enum VehicleStates
        {
            Parked,
            Idling,
            FirstGear,
            SecondGear
        }
    }
}
