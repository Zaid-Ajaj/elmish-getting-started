module App

open System
open Elmish
open Elmish.React
open Fable.React 
open Fable.React.Props

type Todo = {
  Id : int
  Description : string
  Completed : bool
}

type State = { 
  TodoList: Todo list 
  NewTodoDescription : string 
}

type Msg =
  | SetNewTodoDescription of string 
  | AddNewTodo 
  | DeleteTodo of int
  | ToggleCompleted of int
  
let init() = { 
  TodoList = [ 
    { Id = 1; Description = "Learn F#"; Completed = false } 
    { Id = 2; Description = "Learn Elmish"; Completed = true } 
  ]
  NewTodoDescription = "" 
}

let update (msg: Msg) (state: State) =
  match msg with
  | SetNewTodoDescription desc -> 
      { state with NewTodoDescription = desc }
  
  | AddNewTodo when String.IsNullOrWhiteSpace state.NewTodoDescription ->
      state 

  | AddNewTodo ->
      let nextTodoId = 
        match state.TodoList with
        | [ ] -> 1
        | elems -> 
            elems
            |> List.maxBy (fun todo -> todo.Id)  
            |> fun todo -> todo.Id + 1

      let nextTodo = 
        { Id = nextTodoId
          Description = state.NewTodoDescription
          Completed = false }
          
      { state with 
          NewTodoDescription = ""
          TodoList = List.append state.TodoList [nextTodo] }

  | DeleteTodo todoId ->
      let nextTodoList = 
        state.TodoList
        |> List.filter (fun todo -> todo.Id <> todoId)
      
      { state with TodoList = nextTodoList }

  | ToggleCompleted todoId ->
      let nextTodoList = 
        state.TodoList
        |> List.map (fun todo -> 
           if todo.Id = todoId 
           then { todo with Completed = not todo.Completed }
           else todo)
 
      { state with TodoList = nextTodoList }
                
let createTodoTextbox state dispatch = 
  div [ Class "field has-addons" ] [
    div [ Class "control is-expanded" ] [ 
      input [ 
        Class "input is-medium"
        valueOrDefault state.NewTodoDescription
        OnChange (fun ev -> dispatch (SetNewTodoDescription ev.Value)) ]
    ] 
    div [ Class "control" ] [ 
      button [ Class "button is-primary is-medium"; OnClick (fun _ -> dispatch AddNewTodo) ] [ 
        i [ Class "fa fa-plus" ] [ ]
      ]
    ] 
  ] 

let renderTodo (todo: Todo) (dispatch: Msg -> unit) = 
  let checkButtonStyle = 
    classList [ 
      "button", true
      "is-success", todo.Completed
      "is-outlined", not todo.Completed 
    ]
    
  div [ Class "box" ] [ 
    div [ Class "columns is-mobile" ] [ 
      div [ Class "column" ] [
        p [ Class "subtitle is-4" ] [ str todo.Description ] 
      ]
      div [ Class "column" ] [
        div [ Class "buttons is-right" ] [
          button [ checkButtonStyle; Style [ Margin 5 ]; OnClick(fun _ -> dispatch (ToggleCompleted todo.Id))  ] [
            i [ Class "fa fa-check" ] [ ] 
          ] 
          
          button [ Class "button is-danger"; Style [ Margin 5; ]; OnClick (fun _ -> dispatch (DeleteTodo todo.Id)) ] [ 
            i [ Class "fa fa-times" ] [ ] 
          ] 
        ]
      ]
    ]
  ]  

let render (state: State) (dispatch: Msg -> unit) =
  div [ Style [ Padding 20 ] ] [
    h3 [ Class "title" ] [ str "To-Do list" ]
    createTodoTextbox state dispatch
    div [ Class "content"; Style [ MarginTop 20 ] ] [ 
      for todo in state.TodoList -> renderTodo todo dispatch
    ]
  ]

Program.mkSimple init update render
|> Program.withReactSynchronous "elmish-app"
|> Program.withConsoleTrace
|> Program.run