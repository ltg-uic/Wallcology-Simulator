// using UnityEngine;
// using System.Collections;

// public class FlightState: ICritterState
// {

//     public FlightState(StatePatternCritter activeCritter)
//     {
//         critter = activeCritter;
//     }

//     private void Flee()
//     {

//     }

//     public void UpdateState()
//     {
//         Look();
//         Flee();
//     }

//     public void OnTriggerEnter (Collider other) {}

//     public void Look() {}

//     public void ToWanderState() {}

//     public void ToIdleState() {}

//     public void ToForageState() {}

//     // public void ToPursueState()
//     public void ToFlightState() {}

//     public void HandlePredator( StatePatternCritter predator ) {}

//     public void HandleHerbivore( StatePatternCritter herbivore ) {}

//     public void HandleResource( GameObject plant ) {}

// }