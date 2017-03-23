using System;
using System.IO;
using Windows.Storage;

namespace PlantDB.UWP
{
    class FileAccessHelper
    {

        public static string GetLocalFilePath(string fileName)
        {
            //Localfolder should be a place where the app can write files
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            string path = localFolder.Path;
            string dbPath = Path.Combine(path, fileName);

            //If this is the first run, then copy the database out of the app so we have something to work with.
            //If the database already exists, then this function does nothing
            //I should pass in the localFolder, but that is causing an error so going to manually keep these methods in sync
            CopyDatabaseIfNotExistsAsync(fileName);

            return dbPath;
        }

        //should this be static?
        private static async void CopyDatabaseIfNotExistsAsync(string fileName)
        {
            StorageFolder targetFolder = ApplicationData.Current.LocalFolder;

            //We have a couple checks to make to ensure that the file needs to be copied, so track that state in a bool
            bool needToCopy = false;

            //check if the file exists at all, or it exists and is zero bytes/the wrong size
            if (await targetFolder.TryGetItemAsync(fileName) != null)
            {
                //check the size to make sure it's non-zero
                StorageFile file = await StorageFile.GetFileFromPathAsync(Path.Combine(targetFolder.Path, fileName)).AsTask().ConfigureAwait(false);
                Windows.Storage.FileProperties.BasicProperties props = await file.GetBasicPropertiesAsync().AsTask().ConfigureAwait(false);

                //If file is zero bytes, then an error probably occurred in the past, so we should stomp it
                if (props.Size == 0)
                {
                    await file.DeleteAsync().AsTask().ConfigureAwait(false);
                    needToCopy = true;
                }
            }
            else
            {
                needToCopy = true;
            }

            if (needToCopy)
            {
                try
                {
                    //First, get the file out of the app package
                    Uri sourceURI = new Uri("ms-appx:///" + fileName, UriKind.Absolute);
                    StorageFile sourceFile = await StorageFile.GetFileFromApplicationUriAsync(sourceURI).AsTask().ConfigureAwait(false);

                    //Second, create the file we want to write to
                    StorageFile targetFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName).AsTask().ConfigureAwait(false);

                    //Copy it
                    using (Stream sourceStream = await sourceFile.OpenStreamForReadAsync().ConfigureAwait(false))
                    {
                        using (Stream targetStream = await targetFile.OpenStreamForWriteAsync().ConfigureAwait(false))
                        {
                            await sourceStream.CopyToAsync(targetStream);
                        }
                    }
                }

                catch
                {
                    //Something went wrong, need to put in some error handling eventually
                    ;
                }
            }

        }

    }
}
