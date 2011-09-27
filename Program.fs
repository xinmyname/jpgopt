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

let parseArgs(args:string[]) = 
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

    options


let optimizeJpeg(options : Options) = 
    let image = Image.FromFile(options.InputFile)

    let newHeight = (options.Width * image.Height) / image.Width

    let result = new Bitmap(options.Width, newHeight)

    use g = Graphics.FromImage(result)
    g.CompositingQuality <- Drawing2D.CompositingQuality.HighQuality
    g.InterpolationMode <- Drawing2D.InterpolationMode.HighQualityBicubic
    g.SmoothingMode <- Drawing2D.SmoothingMode.HighQuality
    g.DrawImage(image, 0, 0, options.Width, newHeight)

    image.Dispose()

    result

                             
let saveJpeg(result : Image, options : Options) =
    let jpegEncoder = 
        ImageCodecInfo.GetImageEncoders() 
        |> Seq.find (fun e -> e.FormatID = ImageFormat.Jpeg.Guid)

    let qualityParam = new EncoderParameter(Encoder.Quality, int64 options.Quality)

    let encoderParams = new EncoderParameters(1)
    encoderParams.Param.[0] <- qualityParam

    result.Save(options.OutputFile, jpegEncoder, encoderParams)
                                 
                                   
[<EntryPoint>]
let main(args:string[]) =
    printfn "Makes JPEG files a little more managable"

    let options = parseArgs(args)
    let result = optimizeJpeg(options)
    saveJpeg(result, options)
    
    0