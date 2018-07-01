[![Build status](https://ci.appveyor.com/api/projects/status/0l0bjmkcv1hihmq3?svg=true)](https://ci.appveyor.com/project/Yitzchok/zmanim)

This project is a port from the [Java zmanim-project](http://www.kosherjava.com/zmanim-project/) developed by Eliyahu Hershfeld.

The _Zmanim_ ("times" referring to the calculations of time that govern the start and end time of Jewish prayers and holidays)
project is a .NET API for generating zmanim from within .NET programs.
If you are a non programmer, this means that the software created by the project is a building block of code to allow other programmers to easily include zmanim in their programs.
The basis for most zmanim in this class are from the sefer Yisroel Vehazmanim by Rabbi Yisroel Dovid Harfenes.

The code available under the LGPL license.
Please note: due to atmospheric conditions (pressure, humidity and other conditions), calculating zmanim accurately is very complex.
The calculation of zmanim is dependant on Atmospheric refraction (refraction of sunlight through the atmosphere), and zmanim can be off by up to 2 minutes based on atmospheric conditions.
Inaccuracy is increased by elevation. It is not the intent of this API to provide any guarantee of accuracy.


TODO:
    * Make it Linq friendly.
    * Add examples how to use this project in a ASP.NET MVC site and WPF Application.