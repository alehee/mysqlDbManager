# mysqlDbManager
![It's a front pic!](https://github.com/alehee/mysqlDbManager/blob/main/git_res/banner.png?raw=true)

## Description
The *mysqlDbManager* is a small project which can simplify your basic MySQL database management. It's created to do simply operations on tables: check data, insert some rows, update existing rows or just send your queries to the database! Includes fast table export to JSON or Excel file! 

Features:
* last session connection data save
* easy and fast table refresh
* edit specific cell by double-click on it
* GUI window for UPDATE/INSERT queries
* table data export to JSON/Excel

*It might come in handy ;)* ~ Me, 2021

<p align="center">
  <img src="https://github.com/alehee/mysqlDbManager/blob/main/git_res/main_window.png">
</p>

## Used technology
Technology I used for this project:
* C#
* C# WPF
* .NET Framework 4.7.2
* MySQL

And some nuggets like:
* DocumentFormat.OpenXml
* MySql.Data
* Newtonsoft.Json
* Ookii.Dialogs.Wpf

## Installation
There's two ways: you can download the master branch with code, check how it's working and compile whole application in *Visual Studio 2019*, or simply download zipped directory from link below.

  ### Requirements
  * Windows 10
  * .NET SDKs for .NET applications
  
  ### Download
  Download [here](https://drive.google.com/file/d/1apd9ZUVCcg0AX6dNgcALZR3DgH4PVOfE/view?usp=sharing) latest version, unzip it and run *mysqlDbManager.exe*.

## How to use
After download unzip folder, and run the *mysqlDbManager.exe*. Next step is to log into the database and from now on you can manage your database!

Here's a short description of main window features:

<p align="center">
  <img src="https://github.com/alehee/mysqlDbManager/blob/main/git_res/main_window_desc.png">
</p>

Short description of the navbar options:
* ![Simple color](https://dummyimage.com/10x10/ff0000/ff0000) **Custom query** - textbox and button for custom queries
* ![Simple color](https://dummyimage.com/10x10/ffff00/ffff00) **Edit data window** - window for with GUI for UPDATE/INSERT queries
* ![Simple color](https://dummyimage.com/10x10/00ff00/00ff00) **Table select** - table select option and refresh button for table
* ![Simple color](https://dummyimage.com/10x10/0000ff/0000ff) **Table view** - space where table is displayed and you can modify specific cell
* ![Simple color](https://dummyimage.com/10x10/6600ff/6600ff) **Log** - event log for the program
* ![Simple color](https://dummyimage.com/10x10/663300/663300) **Version** - version number and little cheese for you! :)

*Edit: I added table export to JSON and Excel with two more buttons next to event log, you won't overlook it*

Application is easy to use so feel free to test it!

## Changelog
Fixes/New functions/Changes in the program

* **1.1.0** - *26.03.2021*
  * added export to JSON
  * added export to Excel Spreadsheet
  * added few more exception handlers

* **1.0.0** - *23.03.2021*
  * full version released

## Thank you!
Thank you for peeking at my project!

If you're interested check out my other stuff [here](https://github.com/alehee)
