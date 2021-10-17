[<AutoOpen>]
module Css

open Zanaptak.TypedCssClasses

type FA = CssClasses<"../node_modules/@fortawesome/fontawesome-free/css/all.min.css", Naming.PascalCase>
type Bulma = CssClasses<"../node_modules/bulma/css/bulma.min.css", Naming.PascalCase>
