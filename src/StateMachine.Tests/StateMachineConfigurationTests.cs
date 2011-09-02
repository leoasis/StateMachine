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

                    cfg.OnEvent("Park").Transition
                        .From(VehicleStates.Idling, VehicleStates.FirstGear)
                        .To(VehicleStates.Parked);

                    cfg.OnEvent("Ignite")
                        .Transition.From(VehicleStates.Stalled).ToSame()
                        .Transition.From(VehicleStates.Parked).To(VehicleStates.Idling); 

                    cfg.OnEvent("Idle")
                        .Transition.From(VehicleStates.FirstGear).To(VehicleStates.Idling);
   
                    cfg.OnEvent("ShiftUp")
                        .Transition.From(VehicleStates.Idling).To(VehicleStates.FirstGear)
                        .Transition.From(VehicleStates.FirstGear).To(VehicleStates.SecondGear)
                        .Transition.From(VehicleStates.SecondGear).To(VehicleStates.ThirdGear);

                    cfg.OnEvent("ShiftDown")
                        .Transition.From(VehicleStates.ThirdGear).To(VehicleStates.SecondGear)
                        .Transition.From(VehicleStates.SecondGear).To(VehicleStates.FirstGear);
       
                    cfg.OnEvent("Crash").Transition
                        .From(States<VehicleStates>.All - new[] {VehicleStates.Parked, VehicleStates.Stalled})
                        .To(VehicleStates.Stalled)
                        .Unless(x => x.AutoShopBusy);
                });
            }

            public void Crash()
            {
                State.Fire("Crash");
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
            public bool AutoShopBusy { get; set; }
        }

        private enum VehicleStates
        {
            Parked,
            Idling,
            FirstGear,
            SecondGear,
            ThirdGear,
            Stalled
        }
    }
}
