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
  EditModel : (int * string) option
}

type Msg =
  | SetNewTodoDescription of string 
  | AddNewTodo 
  | DeleteTodo of int
  | ToggleCompleted of int
  | CancelEdit
  | ApplyEdit
  | StartEditModel of int 
  | EditModelModified of string 

  
let init() = { 
  TodoList = [ 
    { Id = 1; Description = "Learn F#"; Completed = false } 
    { Id = 2; Description = "Learn Elmish"; Completed = true } 
  ]
  NewTodoDescription = ""
  EditModel = None
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

  | StartEditModel todoId -> 
      let nextEditModel = 
        match state.EditModel with 
        | None -> Some (todoId, "")
        | Some (_, currentEditText) -> 
            state.TodoList
            |> List.tryFind (fun todo -> todo.Id = todoId)
            |> Option.map (fun todo -> todoId, todo.Description)     
      
      { state with EditModel = nextEditModel } 

  | CancelEdit -> 
      { state with EditModel = None }
  
  | ApplyEdit -> 
      match state.EditModel with 
      | None -> state 
      | Some (_, "") -> state 
      | Some (todoId, editText) -> 
          let nextTodoList = 
            state.TodoList
            |> List.map (fun todo -> 
                if todo.Id = todoId
                then { todo with Description = editText }
                else todo)
          
          { state with TodoList = nextTodoList; EditModel = None }

  | EditModelModified newText -> 
      let nextEditModel = 
        state.EditModel
        |> Option.map (fun (id, oldText) -> id, newText) 
      
      { state with EditModel = nextEditModel }


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

let renderEditModel (state: State) (todo: Todo) (dispatch: Msg -> unit) = 
  let editText = 
    state.EditModel 
    |> Option.map snd 
    |> Option.defaultValue ""

  div [ Class "box" ] [
    div [ Class "columns is-mobile" ] [ 
      div [ Class "column" ] [
        input [ Class "input is-medium"; valueOrDefault editText; OnChange (fun ev -> dispatch (EditModelModified ev.Value)) ]
      ]
      div [ Class "column" ] [
        div [ Class "buttons is-right" ] [
          button [ Class "button is-info"; Style [ Margin 5 ]; OnClick (fun _ -> dispatch ApplyEdit)  ] [
            i [ Class "fa fa-save" ] [ ] 
          ] 
          
          button [ Class "button is-danger"; Style [ Margin 5; ]; OnClick (fun _ -> dispatch CancelEdit) ] [ 
            i [ Class "fa fa-arrow-right" ] [ ] 
          ] 
        ]
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
          button [ Class "button is-info"; Style [ Margin 5 ]; OnClick (fun _ -> dispatch (StartEditModel todo.Id))  ] [
            i [ Class "fa fa-edit" ] [ ] 
          ] 
          button [ Class "button is-danger"; Style [ Margin 5; ]; OnClick (fun _ -> dispatch (DeleteTodo todo.Id)) ] [ 
            i [ Class "fa fa-times" ] [ ] 
          ] 
        ]
      ]
    ]
  ]  

let renderTodoItem (state: State) (todo: Todo) (dispatch: Msg -> unit) = 
  match state.EditModel with 
  | Some (todoId, _) when todoId = todo.Id -> renderEditModel state todo dispatch
  | otherwise -> renderTodo todo dispatch 

let render (state: State) (dispatch: Msg -> unit) =
  div [ Style [ Padding 20 ] ] [
    h3 [ Class "title" ] [ str "To-Do list" ]
    createTodoTextbox state dispatch
    div [ Class "content"; Style [ MarginTop 20 ] ] [ 
      for todo in state.TodoList -> renderTodoItem state todo dispatch
    ]
  ]

Program.mkSimple init update render
|> Program.withReactSynchronous "elmish-app"
|> Program.withConsoleTrace
|> Program.run