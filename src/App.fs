module App

open Elmish
open Elmish.React
open Fable.Helpers.React.Props
open Fable.Helpers.React 
 
// State holds the data that you want to keep track of while your application is running
type State = { Count: int }

// Messages are the event to which you react, whenever an event is triggered (i.e. dispatched),
// we compute the next state based on the triggered event
type Msg = 
    | Increment
    | Decrement


let init() = { Count = 0 }, Cmd.none 

// the update function defines the logic of computing the next state based on the incoming message (triggerd event)
let update msg state = 
    match msg with 
    | Increment -> 
        let nextState = { state with Count = state.Count + 1 }
        nextState, Cmd.none

    | Decrement ->
        let nextState = { state with Count = state.Count - 1 }
        nextState, Cmd.none

// The render function creates the user interface based on the current state
// it can bind event handlers to UI elements (like a click on a button) 
// and have them dispatch a message that triggers the update function 
let render state dispatch = 
    let makeButton msg title = 
        button [ OnClick (fun _ -> dispatch msg)
                 Style [ Margin 10 ] ] 
               [ str title ]
    
    div [ ] [ 
        makeButton Increment "Increment"
        makeButton Decrement "Decrement"
        h2 [ ] [ str (sprintf "Current count is %d" state.Count) ]
    ]


// combine init, update and render into a "Program" and run it
Program.mkProgram init update render
|> Program.withReact "root"
|> Program.run