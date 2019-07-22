Imports System.IO

Module Module1

    Sub Main()
        MyParser.Setup()

        Dim fileReader As String
        fileReader = My.Computer.FileSystem.ReadAllText(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "entrada.txt"))

        If MyParser.Parse(New StringReader(fileReader)) Then
            Console.WriteLine()
            Console.WriteLine("Ejecutado Exitosamente")
        Else
            Console.WriteLine("No se pudo ejecutar")
        End If

        Console.ReadLine()

    End Sub

End Module
