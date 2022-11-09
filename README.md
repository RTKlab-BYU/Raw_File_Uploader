# Raw file uploader
Raw file uploader is a upload tool to be used with Proteomics Data Manager (https://github.com/xiaofengxie128/Proteomic-Data-Manager) for uploading raw files to the database with metadata. The program is built on C# with .net 4.5.1 (same framework as Thermo Orbitrap Tribride) for Windows platform.


# Initial Configuration
First, put in your server location to the box provided on the Server tab.
![image](https://user-images.githubusercontent.com/26573132/200887576-8a94b7f7-84db-4e7b-9850-b7467a79edc7.png)

Next, input the login information from your Proteomics Data Manager that you would like to use for this set of uploads.
![image](https://user-images.githubusercontent.com/26573132/200887702-8a7b120b-05d2-44f6-b431-979684e2efd0.png)

Next, verify your login and server information by clicking the 'Verify Account' button.
![image](https://user-images.githubusercontent.com/26573132/200887810-57c12e76-c753-4167-beea-6ab0adeb22f2.png)
![image](https://user-images.githubusercontent.com/26573132/200888383-dcbb64a4-a1e7-46a2-975f-780f408fd26f.png)

Next, you can send alert emails when the MS has stopped acquiring (for example, due to an error) by putting your email into the 'Alert Recipient Email' and setting the max alert interval to the length of your acquisition method. Additionally, you can set a limit on the number of emails you recieve in a given time. Finally, you can configure the sending email using the 'Config SMTP Server' button and the dialog box.
![image](https://user-images.githubusercontent.com/26573132/200888685-498d3375-f08b-4129-9594-323e9714f80a.png)


Finally, The No Copy mode box should generally NOT be checked because if off busy files will not be uploaded and may cause an error, but if you don't want to make temporary copies before uploading you can check this box.
![image](https://user-images.githubusercontent.com/26573132/200888821-13cc891f-89a7-4101-9d25-0b2a13f7ebe2.png)

To save your settings or load old settings click on the 'File' menu in the top left corner.
![image](https://user-images.githubusercontent.com/26573132/200889326-fb682343-cc56-4aa9-ba4d-efd7e7547702.png)


# Uploading & Monitoring

First, confirm that the login information matches that of the person who owns the acquisition files (don't forget to verify your information).
![image](https://user-images.githubusercontent.com/26573132/200889836-c2cccf59-3989-4a53-8b63-fd5419019422.png)

Select, the file Type in the Files tab
![image](https://user-images.githubusercontent.com/26573132/200891671-0036e3d5-f034-4ea0-8214-adc9244de8d5.png)

Then you need to choose the files to upload, by using one of the following three methods (see appendix 1 for anything other than Thermo Raw files)

## 1.A upload a file
First, browse or type a filename (or files separated by '.'), then click 'manual upload files/folder'
![image](https://user-images.githubusercontent.com/26573132/200890974-330123ea-6b47-44cc-99f9-fed62c797d5a.png)

## 1.B upload folder based data
First, browse or type a folder location (or add folders separated by '.'), then click 'manual upload files/folder'
![image](https://user-images.githubusercontent.com/26573132/200892660-25affbb9-a508-4321-aa26-2bfbcab98dec.png)


## 2. upload all files (or subfolders if MS data is stored in folders)
First, browse or type a folder location (or files separated by '.'), then click 'manual upload files/folder'
![image](https://user-images.githubusercontent.com/26573132/200893339-259a2ae6-e4d4-4482-9f6d-64813ab6f5cd.png)

## 3. monitor a folder for any new data, and automatically upload data
First, maximum and minimum file sizes can be determined on the Settings tab, as well as a pattern in the filename that you don't want uploaded (such as 'blank').
![image](https://user-images.githubusercontent.com/26573132/200889552-4eba4ac6-cc42-4e8f-b5e3-d9baddc51f93.png)

As files are uploaded, they will be reported in the log
![image](https://user-images.githubusercontent.com/26573132/200893439-c126edb5-d2e5-4847-8551-71a23fe26600.png)


# Appendix 1: Using other file formats
![image](https://user-images.githubusercontent.com/26573132/200893928-30645240-413c-4724-8cf4-37530673e63f.png)
To use other file formats than Thermo Raw, you will not to do some configuration:
First select the file type configure in the dropdown.
Next the file extension (i.e. raw for thermo raw files and .d for Agilent folders)
Next is the Acquisition programs name. This is the program used to make the raw files, raw file uploader waits until this program is done with the file before uploading the file, so that partial files aren't uploaded. 
This can be found by using the 'File_Lock_Checker' in the tools menu.
![image](https://user-images.githubusercontent.com/26573132/200896911-5fed90b5-0aaf-4ade-88e3-2837b835aef8.png)

Next, you select if it is folder based data, and then how long to delay before upload. Because some acquisition files may not be busy for the whole time after the creation, you may want to set this to the length of the run for some systems. For folder based data, the Final file is the last file created or the file the aquisition program keeps busy or creates to trigger your upload (or wait). 
![image](https://user-images.githubusercontent.com/26573132/200897278-5c99ad2b-6c39-4398-b151-5ce530bc78f0.png)


# Appendix 2: Adding metadata
Metadata can either be automatically extracted from the filename or defined manually for each upload
![image](https://user-images.githubusercontent.com/26573132/200898020-ee3b1b02-4459-473f-b45c-3b497e56ae6e.png)

First, define your column and spe serial numbers and sample type for this upload
![image](https://user-images.githubusercontent.com/26573132/200898274-3ed26696-8a28-48c6-9f8b-f6399a372b4b.png)

Next, fill in the table (which is split into two) for any information pulled from the file name. For example if your fields are delimited by underscores than a file named mouse_brain.raw, could have metadata extracted by making the first factor name '\[0]' and the first factor value "organism" and the second pair '\[1]' and "organ".
![image](https://user-images.githubusercontent.com/26573132/200899184-97800ae5-a8d3-44e7-af2b-26d80dcd0750.png)

Finally upload your data as described above.



