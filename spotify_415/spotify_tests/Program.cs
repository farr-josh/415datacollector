// See https://aka.ms/new-console-template for more information
using Microsoft.VisualBasic.FileIO;
using spotify_tests;
using spotify_tests.API_ResponseModels.Artists;
using spotify_tests.API_ResponseModels.Albums;
using System;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;


//start of application
SpotifyClient client = new SpotifyClient();
DatasetCreator dataCreator = new DatasetCreator();
PathSetter pathsetter = new PathSetter();

//dataset paths
string readPath = pathsetter.AppendWorkingPath("/DatasetsForGetting/ArtistIDs.csv");
string writePath = pathsetter.AppendWorkingPath("/FinalDatasets/Artists.csv");
string RejectPath = pathsetter.AppendWorkingPath("/Rejected/Artists.csv");


/// <summary>
/// for every artist id batch in /DatasetsForGetting/ArtistIDs.csv
/// call https://api.spotify.com/v1/artists with param id batch
/// store the json response into GetArtistResponse model
/// extract values from response (artistID,name,followers,popularity,genres)
/// ensure no duplicates are made using dictionary
/// write the unique key and values from dictionary to Artists.csv
/// </summary>
//string ids = "";
using (var sw = new StreamWriter(writePath))
{
    using (TextFieldParser csvParser = new TextFieldParser(readPath))
    {
        while (!csvParser.EndOfData)
        {
            //read each artist ID batch
            ids = csvParser.ReadLine();
            //Call the Spotify api
            string response = client.GetArtistByIDS(ids);

            if (response != string.Empty)
            {
                //store the response
                dataCreator.AddArtists(response);
            }
            else
            {
                using (var swReject = new StreamWriter(RejectPath))
                {
                    swReject.WriteLine(ids);
                    swReject.Flush();
                }
            }
        }
    }
    //write all the data to file
    sw.WriteLine("ID,name,followers,popularity,genres");
    dataCreator.WriteArtists(sw);
}



/// <summary>
/// similar to the previous method of getting artists.
/// From the Artist ids and names we just collected, get all albums featuring the artists
/// read ids from /FinalDatasets/Artists.csv
/// call the api for albums
/// write the response to /FinalDatasets/Albums.csv
/// </summary>

//dataset paths
readPath = pathsetter.AppendWorkingPath("/FinalDatasets/Artists.csv");
writePath = pathsetter.AppendWorkingPath("/FinalDatasets/Albums.csv");
string albumID;
string[] fields;

//batch size limiter (5000 ids per batch)
int i = 0;

//counter
int ids_searched_count = 0;

//pagination variables
int offset;
int limit;
bool paginate = true;

using (var sw = new StreamWriter(writePath))
{
    //add csv file header
    sw.WriteLine("id, name, release date, type, group, [contributing artist ids]");
    using (TextFieldParser csvParser = new TextFieldParser(readPath))
    {
        csvParser.SetDelimiters(new string[] { "," });
        csvParser.ReadFields();

        //read one artist ID at a time
        while (!csvParser.EndOfData & i != 5000)
        {
            //~1 hour to call 5000 ids
            //new token every 900 ids = new token every ~11 minutes (max token life is 30 miutes)
            if (i >= 900)
            {
                Console.WriteLine("refreshing token");
                client.GetNewToken();
                System.Threading.Thread.Sleep(300);
                i = 0;
            }

            //parse Artists.csv to get artist id
            fields = csvParser.ReadFields();
            albumID = fields[0];

            //reset pagination
            offset = 0;
            limit = 50;
            while (paginate)
            {
                //trying to avoid rate limit by slowing down
                System.Threading.Thread.Sleep(300);

                //get the response as json string
                string response = client.GetAlbumsByArtistID(albumID, offset, limit);

                if (response != string.Empty)
                {
                    paginate = dataCreator.AddAlbum(response, sw);
                }
                //get next page
                offset += limit;
            }
            //display what row in Artists.csv the program is at
            ids_searched_count++;
            Console.WriteLine("The Number of ids searched: " + ids_searched_count);
            i++;
            paginate = true;
        }

        //write the batch
        dataCreator.WriteAlbums(sw);
    }
}



/// <summary>
/// read album id's from /FinalDatasets/Albums.csv
/// call the api for all tracks in the album
/// write the response to /FinalDatasets/Tracks.csv
/// </summary>

readPath = pathsetter.AppendWorkingPath("/FinalDatasets/Albums.csv");
writePath = pathsetter.AppendWorkingPath("/FinalDatasets/Tracks.csv");

int skipper = 0;
i = 0;
ids_searched_count = 0;
paginate = true;
string line = "";
using (var sw = new StreamWriter(writePath))
{
    using (TextFieldParser csvParser = new TextFieldParser(readPath))
    {
        //while (skipper != 50000)//read 50,001 to 100,001
        //{
        //    line = csvParser.ReadLine();
        //    skipper++;
        //}
        //Console.WriteLine(line);
        //return;

        while (!csvParser.EndOfData & ids_searched_count != 50000)
        {
            //token refresher
            if (i == 600)
            {
                Console.WriteLine("refreshing token");
                client.GetNewToken();
                System.Threading.Thread.Sleep(300);
                i = 0;
            }

            //read id
            line = csvParser.ReadLine();
            albumID = line.Substring(0, 22);
            Console.WriteLine("ID read");
            limit = 50;
            offset = 0;
            while (paginate)
            {
                //geting 400 tracks every 2 minutes
                System.Threading.Thread.Sleep(150);
                string response = client.GetTracksByAlbumID(albumID, offset, limit);

                if (response != string.Empty)
                {
                    paginate = dataCreator.AddTrack(response, albumID);
                }
                offset += limit;
            }
            paginate = true;
            i++;
            ids_searched_count++;
            Console.WriteLine("CURRENT COUNT IS: " + ids_searched_count);
        }
        sw.WriteLine("header");
        dataCreator.WriteTracks(sw);
    }
}



/// <summary>
/// This is used to clean the /FinalDatasets/Albums.csv
/// check for duplicates and errors parsing the csv
/// </summary>

readPath = pathsetter.AppendWorkingPath("/FinalDatasets/Albums.csv");
writePath = pathsetter.AppendWorkingPath("/FinalDatasets/AlbumsCleaned.csv");

IDictionary<string, string> Albums = new Dictionary<string, string>();
int dupes = 0;
using (TextFieldParser csvParser = new TextFieldParser(readPath))
{
    csvParser.ReadLine();
    while (!csvParser.EndOfData)
    {
        //string[] fields = null;

        string field = csvParser.ReadLine();
        albumID = field.Substring(0, 22);
        if (!Albums.ContainsKey(albumID))
        {
            Albums.Add(albumID, field.Substring(22, field.Length - 22));
            Console.WriteLine(albumID + field.Substring(22, field.Length - 22));
        }
        else
        {
            dupes++;
        }
    }
}
Console.WriteLine("THERE WERE: " + dupes + " DUPES");
Console.WriteLine("COMPLETE");
using (var sw = new StreamWriter(writePath))
{
    foreach (KeyValuePair<string, string> alb in Albums)
    {
        sw.WriteLine(alb.Key + alb.Value);
        sw.Flush();
    }
}


/// <summary>
/// this is used to clean the tracks.csv
/// NOTE: tracks.csv data is incomplete right now and will take time
/// </summary>
/// 








/// <summary>
/// this is used to get track audio features
/// </summary>


readPath = pathsetter.AppendWorkingPath("/FinalDatasets/Tracks.csv");
writePath = pathsetter.AppendWorkingPath("/FinalDatasets/TracksAudioFeatures.csv");

i = 0;
int count = 0;
paginate = true;
line = "";
string albumIDs = "";
HashSet<string> trackIds = new HashSet<string>();

using (TextFieldParser csvParser = new TextFieldParser(readPath))
{
    csvParser.SetDelimiters(new string[] { "," });
    //read all of the track id's into dict
    while (!csvParser.EndOfData)
    {
        //fields = csvParser.ReadFields();

        line = csvParser.ReadLine();
        albumIDs = line.Split('[', ']')[1];
        fields = albumIDs.Split(',');

        //Console.WriteLine(albumIDs);
        foreach (string field in fields)
        {
            trackIds.Add(field);
        }

    }

}
int sizestopper = 0;
int j = 0;
string[] idsList = trackIds.ToArray();
int length = idsList.Length - 1;
long subsets = length / 100;
Console.WriteLine("THE SIZE IT: " + length);
Console.WriteLine("THE subsets IT: " + subsets);
string batch = "";

for (i = 0; i <= length; i++)
{
    batch += idsList[i] + ",";

    if (j == 100 || i >= length)
    {
        //remove trailing comma
        batch = batch.Remove(batch.Length - 1, 1);
        //from here we can send a batch of 100

        Console.WriteLine(batch);
        string response = client.GetTracksAudioFeatures(batch);
        if (response != string.Empty)
        {
            dataCreator.AddTrackAudioFeatures(response);
        }
        j = 0;
        batch = "";
    }
    j++;
}
using (var sw = new StreamWriter(writePath))
{
    Console.WriteLine("writing");
    sw.WriteLine("id,Energy,Danceability,Instrumentalness,Key,Loudness,Liveness,Speechiness,Tempo,Valence");
    dataCreator.WriteTrackAudioFeatures(sw);
}