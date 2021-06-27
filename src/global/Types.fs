[<AutoOpen>]
module Types

open Zanaptak.TypedCssClasses

type Msg =
  | Increment
  | Decrement

// Bulma classes
type Bulma = CssClasses<"https://cdnjs.cloudflare.com/ajax/libs/bulma/0.7.4/css/bulma.min.css", Naming.PascalCase>

// Font-Awesome classes
type FA =
  CssClasses<"https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css", Naming.PascalCase>
