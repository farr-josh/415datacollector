
from pymongo import MongoClient


url = "localhost:27017"

client = MongoClient(url)

#Testing is the name of the database you created in MongoDBCompass
testdb = client.Testing

#AlbumArtist is the name of a table created by importing csv
AlbumArtist = testdb.AlbumArtist    #AlbumID, AsrtistID

Albums = testdb.Albums              #id, name, release date, type, group

AlbumTracks = testdb.AlbumTracks    #AlbumID, tracksID

Artist = testdb.Artist              #ArtistID, name, followers, popularity

ArtistGenres = testdb.ArtistGenres  #ArtistID, genres

TrackAudio = testdb.TrackAudio      #TrackID, Energy, Dancability, Instrumentalness, Key, Loudness, Liveness, Speechiness, Tempo, Valence


#given a genre
#1. ArtistGenres -> artistID's

#given a artistID
#2. Artist -> artist data

#given a artistID
#3. AlbumArtist -> albumID's

#given a albumID
#4. AlbumTracks -> trackID's

#given a trackID
#5. TrackAudio -> track data



tolerance = .004 #user input: set tolerance = .05
EnergyInput = .8 #user input: Energy = .8
DanceabilityInput = .7

EnergyLT = EnergyInput + tolerance
EnergyGT = EnergyInput - tolerance

DanceabilityLT = DanceabilityInput + tolerance
DanceabilityGT = DanceabilityInput - tolerance

#build query: find all tracks st.                    0.796 > Energy > 0.8004                    AND                         0.696 > DanceabilityGT > 0.7004
TrackAudioResult = TrackAudio.aggregate([{"$match": {"Energy":{"$gte":EnergyGT, "$lt":EnergyLT}}}, {"$match": {"Danceability":{"$gte":DanceabilityGT, "$lt":DanceabilityLT}}}])#for item in result3:

for track in TrackAudioResult:
    print(track)

