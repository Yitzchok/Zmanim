Imports net.sourceforge.zmanim.util
Imports net.sourceforge.zmanim
Imports Zmanim.Extensions

Module Module1

    Sub Main()
        Dim locationName As String = "Lakewood, NJ"
        Dim latitude As Double = 40.09596 'Lakewood, NJ
        Dim longitude As Double = -74.22213 'Lakewood, NJ
        Dim elevation As Double = 0 'optional elevation
        Dim timeZone As java.util.TimeZone = java.util.TimeZone.getTimeZone("America/New_York")
        Dim location As New GeoLocation(locationName, latitude, longitude, elevation, timeZone)
        Dim zc As New ComplexZmanimCalendar(location)

        System.Console.WriteLine("Today's Zmanim for " & locationName)
        System.Console.WriteLine("Sunrise: " & zc.getSunrise().ToString)
        'output sunrise
        System.Console.WriteLine("Sof Zman Shema MGA: " & zc.getSofZmanShmaMGA().ToString)
        'output Sof Zman Shema MGA
        System.Console.WriteLine("Sof Zman Shema GRA: " & zc.getSofZmanShmaGRA().ToString)
        'output Sof Zman Shema GRA
        System.Console.WriteLine("Sunset: " & zc.getSunset().ToString)
        'output sunset
        System.Console.WriteLine("Press enter to exit.")
        System.Console.ReadLine()

    End Sub

End Module
