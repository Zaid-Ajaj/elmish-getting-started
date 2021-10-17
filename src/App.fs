module App

open Elmish
open Elmish.React
open Feliz

type State = { Count : int }

let init () = { Count = 22 }

let update (msg : Msg) (state : State) : State =
  match msg with
  | Decrement -> { state with Count = state.Count + 1 }
  | Increment -> { state with Count = state.Count - 1 }

let render (state : State) (dispatch : Msg -> unit) =
  Html.div
    [
      Html.button
        [
          prop.onClick (fun _ -> dispatch Increment)
          prop.classes [ Bulma.IsPrimary ]
          prop.children
            [
              Html.i [ prop.classes [ FA.Fa; FA.FaPlus ] ]
            ]

        ]

      Html.button
        [
          prop.onClick (fun _ -> dispatch Decrement)
          prop.classes [ Bulma.IsPrimary ]
          prop.children
            [
              Html.i [ prop.classes [ FA.Fa; FA.FaMinus ] ]
            ]
        ]

      Html.h1 state.Count
    ]

Program.mkSimple init update render
|> Program.withReactSynchronous "elmish-app"
|> Program.run
