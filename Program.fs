#light

open System
open NDesk.Options
open System.Drawing
open System.Drawing.Imaging

type Options() = 
    let mutable _help = false
    let mutable _quality = 60
    let mutable _width = 1024
    let mutable _inputFile = ""
    let mutable _outputFile = ""

    member this.Help with get() = _help
                     and  set v = _help <- v

    member this.Quality with get() = _quality
                        and  set v = _quality <- v

    member this.Width with get() = _width
                      and  set v = _width <- v

    member this.InputFile with get() = _inputFile
                          and  set v = _inputFile <- v
                               
    member this.OutputFile with get() = _outputFile
                           and  set v = _outputFile <- v
                                   
[<EntryPoint>]
let main(args:string[]) =
    printfn "Makes JPEG files a little more managable"

    let options = new Options()

    let optionSet = new OptionSet()

    optionSet.Add("h|?|help", "Show help message for usage", 
        fun v -> options.Help <- true) |> ignore

    optionSet.Add("q|quality=", "Specified quality level from 0 to 100, default is 60",
        fun v -> options.Quality <- v) |> ignore

    optionSet.Add("w|width=", "Set the maxmimum width of the output image, default is 1024",
        fun v -> options.Width <- v) |> ignore

    optionSet.Add("i|inputFile=", "Input image file",
        fun v -> options.InputFile <- v) |> ignore

    optionSet.Add("o|outputFile=", "Output image file",
        fun v -> options.OutputFile <- v) |> ignore

    optionSet.Parse(args) |> ignore

    if String.IsNullOrEmpty(options.OutputFile) then 
        options.OutputFile <- options.InputFile

    let image = Image.FromFile(options.InputFile)

    let qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, byte options.Quality)

//    ... and here's where I'm stuck
//
//    let jpegEncoder = ImageCodecInfo.GetImageEncoders().Where(<@fun x -> x.FormatID == ImageFormat.Jpeg.Guid@>)

    0

