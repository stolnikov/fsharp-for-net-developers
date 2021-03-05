open MarkdownHelpers.TableOfContents
open MarkdownHelpers.MarkdownMerge
open Chapters
open System.IO

[<EntryPoint>]
let main argv =
    let chaptersDirectory = "./chapters"
    let extension = "md"

    let validChapterPaths =
        chapters
        |> List.map (toRelativePath chaptersDirectory extension)
        |> List.filter File.Exists

    trimFileContents validChapterPaths

    let allTextLines = bodyLines validChapterPaths

    let header = "# Pragmatic guide to F#

📝 author's personal opinion on controversial topics

📙 source code link

The document has numerous references to [useful resources](https://fsharp.org/testimonials/) to save a reader some googling time.

### Table of contents
"

    let tocPattern =
        @"^(?<Level>[#]{1,4}) (?<Title>.{4,128}?)(?<ShouldOmit><!-- omit in toc -->)?$"

    let toc =
        header + compileToc tocPattern allTextLines

    let body = toBodyString allTextLines

    System.IO.File.WriteAllText("README.md", toc + "\n\n" + body)

    0
