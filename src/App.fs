module App

open Elmish
open Elmish.React
open Fable.Helpers.React
open Fable.Helpers.React.Props

type State = { Count: int }

type Msg =
  | Increment
  | Decrement

let init() : State = { Count = 0 }

let update (msg: Msg) (state: State) =
    match msg with
    | Increment -> { state with Count = state.Count + 1 }
    | Decrement -> { state with Count = state.Count - 1 }

let render (state: State) dispatch =
  div []
      [ button [ OnClick (fun _ -> dispatch Increment) ] [ str "+" ]
        div [] [ str (string state.Count) ]
        button [ OnClick (fun _ -> dispatch Decrement) ] [ str "-" ] ]

Program.mkSimple init update render
|> Program.withReact "elmish-app"
|> Program.withConsoleTrace
|> Program.run