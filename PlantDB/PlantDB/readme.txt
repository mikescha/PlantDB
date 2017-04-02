Original code on how to serve up a local database that is shipped with the app came from here: 
http://arteksoftware.com/deploying-a-database-file-with-a-xamarin-forms-app/

Plant normalization steps:
0. Get all the column names to align to the columns in the code. For instance, 
   "Min Height(ft)" --> "MinHeight"
   "Scientific Name" --> "ScientificName"
1. Ensure that months are exactly the twelve months separated by commas. Found a couple separated by periods and one separated by spaces.
2. Ensure that "Annual herb" is changed to "Annual_herb" and same for perennial
3. Ensure that each plant has exactly one Plant Type (unless it's really important that it support multiple, in which I'll change the code)
