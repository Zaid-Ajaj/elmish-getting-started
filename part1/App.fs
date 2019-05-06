module App

open System
open Elmish
open Elmish.React
open Fable.React 
open Fable.React.Props

type State = { 
  TodoList: string list 
  NewTodoDescription : string 
}

type Msg =
  | SetNewTodoDescription of string 
  | AddNewTodo 
  
let init() = { 
  TodoList = [ "Learn F#" ]
  NewTodoDescription = "" 
}

let update (msg: Msg) (state: State) =
  match msg with
  | SetNewTodoDescription desc -> 
      { state with NewTodoDescription = desc }
  
  | AddNewTodo when String.IsNullOrWhiteSpace state.NewTodoDescription ->
      state 

  | AddNewTodo ->
      { state with 
          NewTodoDescription = ""
          TodoList = List.append state.TodoList [state.NewTodoDescription] }

let render (state: State) (dispatch: Msg -> unit) =
  div [ Style [ Padding 30 ] ] [
    h3 [ Class "title" ] [ str "Elmish To-Do list" ]
    // the text box to add new todo items
    div [ Class "field has-addons" ] [
      div [ Class "control is-expanded" ] [ 
        input [ 
          Class "input is-medium"
          valueOrDefault state.NewTodoDescription
          OnChange (fun ev -> dispatch (SetNewTodoDescription ev.Value)) 
        ]
      ] 
      div [ Class "control" ] [ 
        button [ Class "button is-primary is-medium"; OnClick (fun _ -> dispatch AddNewTodo) ] [ 
          i [ Class "fa fa-plus" ] [ ]
        ]
      ] 
    ] 
    // the actual todo items
    ul [ Style [ MarginTop 20 ] ] [ 
      for todo in state.TodoList -> 
      li [ Class "box" ] [ 
        p [ Class "subtitle is-4" ] [ str todo ] 
      ]  
    ]
  ]

Program.mkSimple init update render
|> Program.withReactSynchronous "elmish-app"
|> Program.withConsoleTrace
|> Program.run