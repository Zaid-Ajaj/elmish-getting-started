module App

open Elmish
open Elmish.React
open Feliz

type State =
    { Count: int }

type Msg =
    | Increment
    | Decrement

let init() =
    { Count = 0 }

let update (msg: Msg) (state: State): State =
    match msg with
    | Increment ->
        { state with Count = state.Count + 1 }

    | Decrement ->
        { state with Count = state.Count - 1 }

let render (state: State) (dispatch: Msg -> unit) =
    Html.div [
        prop.id "counter"
        prop.children [
            Html.button [
                prop.onClick (fun _ -> dispatch Increment)
                prop.children [ Html.text "Decrement" ]
            ]

            Html.button [
                prop.onClick (fun _ -> dispatch Decrement)
                prop.children [ Html.text "Decrement" ]
            ]

            Html.h1 state.Count
        ]
    ]

Program.mkSimple init update render
|> Program.withReactSynchronous "elmish-app"
|> Program.run