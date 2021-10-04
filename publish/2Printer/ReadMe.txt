//////////////////// 2Printer ReadMe file ////////////////////
1. The Introduction

2Printer is a command line tool for printing documents and image files in batch mode. With 2printer you are able to automate printing image files, text documents, drawings, worksheets, presentations, PDF files and so on.

2Printer uses an internal graphic engine to print PDF documents and image files such as JPEG, TIFF, PNG, PCX, DCX, Bitmap and TGA. It means that no additional software is required to print them.

2Printer uses public API third party software to print text documents, drawings, etc. As a result, you will need to install and activate Microsoft Word in order to print TXT, RTF, DOC, or DOCX files. Microsoft Visio is required to print VSD files; Autodesk AutoCAD is necessary to print DWG or DXF. Microsoft Excel must be installed to print XLS or XLSX; Microsoft PowerPoint is needed to print PPT or PPTX. 

2Printer prints documents and image files using default settings of the printer you selected. To change default settings:
1) Go to Windows Start menu and click "Devices and Printers".
2) Right click on your selected printer and click "Printing Preferences".
3) Change the defaults and then click "OK".
This will enable 2Printer to adjust to these settings while using the selected printer.

2Printer is compatible to any and all printers that are directly connected to your computer. This includes virtual printers and printers that are connected via local network. 

You can use 2Printer to automate conversion documents to PDF or image files such as JPEG, TIFF, and PNG. Instead of using your office or home printing device, you need to use a virtual printer such as Universal Document Converter.

Thanks to command line interface, you are able to create your own BATCH-files to automate printing documents of your choice. You can also create CMD files and add them to Task Scheduler enabling you to print all documents from a selected folder.

2Printer is free for non-commercial usage. Please order the commercial version in order to use 2Printer for business. 2Printer official web page: https://www.cmd2printer.com


2. 2Printer command line syntax and examples

1) Read 2Printer help file:
2Printer -help

2) Print all documents from folder "C:\Input" on the system default printer:
2Printer -src "C:\Input\*.*"

3) Print all documents from list file "C:\ToDo\input.txt" on the system default printer. The "input.txt" should be a Unicode or ASCII text file with a list of paths to 
document files. Each new file path in this file should be finalized by the Enter key. 
2Printer -src "@C:\ToDo\input.txt"

4) Print all documents from folder "C:\Input" with all subfolders on the system default printer:
2Printer -src "C:\Input\*.*" -options scansf:yes

5) Get full list of available printers:
2Printer -options showprnlist

6) Print all documents from "C:\Input" on the printer "Canon MP610 series Printer":
2Printer -src "C:\Input\*.*" -prn "Canon MP610 series Printer"

7) Print all documents from "C:\Input" on the virtual printer "Universal Document Converter":
2Printer -src "C:\Input\*.*" -prn "Universal Document Converter"

8) Print documents in silent mode:
2Printer -src "C:\Input\*.*" -options silent:yes

9) Edit ini-file with 2Printer defaults:
2Printer -ini