module App

open Elmish
open Elmish.React
open Fable.Helpers.React
open Fable.Helpers.React.Props

type State = { Count: int }

type Msg =
  | Increment
  | Decrement
  
let init() = { Count = 0 }

let update (msg: Msg) (currentState: State) =
  match msg with
  | Increment -> 
      let nextState = { currentState with Count = currentState.Count + 1 }
      nextState

  | Decrement -> 
      let nextState = { currentState with Count = currentState.Count - 1 }
      nextState
      
let render (state: State) (dispatch: Msg -> unit) =
  div []
      [ button [ OnClick (fun _ -> dispatch Increment) ] [ str "+" ]
        div [] [ str (string state.Count) ]
        button [ OnClick (fun _ -> dispatch Decrement) ] [ str "-" ] ]

Program.mkSimple init update render
|> Program.withReact "elmish-app"
|> Program.withConsoleTrace
|> Program.run