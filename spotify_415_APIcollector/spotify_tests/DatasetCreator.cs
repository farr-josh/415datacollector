using spotify_tests.API_ResponseModels;
using spotify_tests.API_ResponseModels.Albums;
using spotify_tests.API_ResponseModels.Artists;
using spotify_tests.API_ResponseModels.byid;
using normal_artist = spotify_tests.API_ResponseModels.Artists.Artist;
using album_artist = spotify_tests.API_ResponseModels.Albums.Artist;
using album_item_byid = spotify_tests.API_ResponseModels.byid.Item;
using album_artist_item_byid = spotify_tests.API_ResponseModels.byid.Artist;
using track_item = spotify_tests.API_ResponseModels.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata.Ecma335;


namespace spotify_tests
{
    public class DatasetCreator
    {
        IDictionary<string, string> artistIDName = new Dictionary<string, string>();
        IDictionary<string, string> Albums = new Dictionary<string, string>();
        IDictionary<string, string> DictTracks = new Dictionary<string, string>();
        HashSet<string> genreTypes = new HashSet<string>();
        string trackids = "";
        string albumAttributes = "";
        string albumArtists = "";
        public DatasetCreator()
        {

        }

        public void AddArtists(string artistJson)
        {
            //artist,artistID,followers,popularity,genres
            GetArtistResponse artists = GetArtistResponse.FromJson(artistJson);
            string artistAttrubtes = "";
            foreach (normal_artist artist in artists.Artists)
            {
                //add name
                artistAttrubtes += artist.Name + ",";
                //add followers
                artistAttrubtes += artist.Followers.Total + ",";
                //add popularity
                artistAttrubtes += artist.Popularity.ToString() + ",";
                //adding genres
                artistAttrubtes += "[";
                foreach (string genre in artist.Genres)
                {
                    genreTypes.Add(genre);
                    if (genre == artist.Genres.Last())
                    {
                        artistAttrubtes += genre;
                    }
                    else
                    {
                        artistAttrubtes += genre + ",";
                    }
                }
                artistAttrubtes += "]";
                if (!artistIDName.ContainsKey(artist.Id))
                {
                    artistIDName.Add(artist.Id, artistAttrubtes);
                }
                artistAttrubtes = "";
            }
        }

        public void WriteArtists(StreamWriter sw)
        {
            foreach (KeyValuePair<string, string> artist in artistIDName)
            {
                sw.WriteLine(artist.Key + "," + artist.Value);
                sw.Flush();
            }
        }

        public bool AddAlbum(string albumJson, StreamWriter sw)
        {
            AlbumsByArtistID albums = AlbumsByArtistID.FromJson(albumJson);
            albumAttributes += "";
            //if type != album return flase

            //id, name, release date, type, group, [contributing artist ids]
            foreach (album_item_byid album in albums.Items)
            {
                albumAttributes += album.Name + ",";
                albumAttributes += album.ReleaseDate + ",";
                albumAttributes += album.Type + ",";
                albumAttributes += album.AlbumGroup + ",";

                //list of contributing artists
                albumAttributes += "[";
                foreach (album_artist_item_byid artist in album.Artists)
                {
                    if (artist == album.Artists.Last())
                    {
                        albumAttributes += artist.Id + "]";
                    }
                    else
                    {
                        albumAttributes += artist.Id + ",";
                    }
                }
                if (!Albums.ContainsKey(album.Id))
                {
                    Albums.Add(album.Id, albumAttributes);
                }
                albumAttributes = "";
            }
            
            Console.WriteLine("total albums: " + Albums.Count());
            if (albums.Next == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void WriteAlbums(StreamWriter sw)
        {
            foreach (KeyValuePair<string, string> album in Albums)
            {
                sw.WriteLine(album.Key + "," + album.Value);
                sw.Flush();
            }
        }


        public bool AddTrack(string trackJson, string albumID)
        {
            TracksByAlbumID tracks = TracksByAlbumID.FromJson(trackJson);
            Console.WriteLine("Adding ids to string");
            foreach (track_item track in tracks.Items)
            {
                if (track.Id != tracks.Items.Last().Id)
                {
                    trackids += track.Id + ",";
                }
                else if(tracks.Next == null)
                {
                    trackids += track.Id;
                }
                else
                {
                    trackids += track.Id + ",";
                }
            }

            if (tracks.Next == null)
            {
                DictTracks.Add(albumID, "[" + trackids + "]");
                this.trackids = "";
                return false;
            }
            else
            {
                return true;
            }
        }

        public void WriteTracks(StreamWriter sw)
        {
            foreach (KeyValuePair<string, string> track in DictTracks)
            {
                sw.WriteLine(track.Key + "," + track.Value);
                sw.Flush();
            }
        }

        public void AddTrackAudioFeatures(string trackAudioFeaturesJson)
        {
            TrackAudioFeatures tracks = TrackAudioFeatures.FromJson(trackAudioFeaturesJson);
            string features = "";
            //id,Energy,Danceability,Instrumentalness,Key,Loudness,Liveness,Speechiness,Tempo,Valence
            foreach (AudioFeature feature in tracks.AudioFeatures)
            {
                features += feature.Energy + "," +
                    +feature.Danceability + "," + feature.Instrumentalness + ","
                    + feature.Key + "," + feature.Loudness + "," + feature.Liveness + ","
                    + feature.Speechiness + "," + feature.Tempo + "," + feature.Valence;
                if (!DictAudio.ContainsKey(feature.Id))
                {
                    DictAudio.Add(feature.Id, features);
                }
                features = "";
            }
        }
        public void WriteTrackAudioFeatures(StreamWriter sw)
        {
            foreach (KeyValuePair<string, string> Audio in DictAudio)
            {
                sw.WriteLine(Audio.Key + "," + Audio.Value);
                sw.Flush();
            }
        }

    }
}
