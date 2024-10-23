using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using RipStainAPI.Models;



public class UploadService
{
    private readonly IMongoDatabase _database;
    private readonly GridFSBucket _gridFSBucket;

    public UploadService(IOptions<RipStainDbSettings> reportDbSettings)
    {
        var mongoClient = new MongoClient(
            reportDbSettings.Value.ConnectionString);
        _database = mongoClient.GetDatabase(reportDbSettings.Value.DatabaseName);
        _gridFSBucket = new GridFSBucket(_database);
          
       
    }

    // Uploads a file to GridFS
    public async Task<ObjectId> UploadFileAsync(Stream fileStream, string fileName, string contentType)
    {
        var options = new GridFSUploadOptions
        {
            Metadata = new BsonDocument
            {
                { "contentType", contentType }
            }
        };

        ObjectId fileId;
        try
        {
            // Upload file to GridFS
            fileId = await _gridFSBucket.UploadFromStreamAsync(fileName, fileStream, options);
        }
        catch (Exception ex)
        {
            // Handle errors here
            throw new Exception("Error while uploading file to GridFS: " + ex.Message);
        }

        return fileId;
    }

    // Download file by id
    public async Task<Stream> DownloadFileAsync(ObjectId fileId)
    {
        var fileStream = new MemoryStream();

        try
        {
            await _gridFSBucket.DownloadToStreamAsync(fileId, fileStream);
            fileStream.Seek(0, SeekOrigin.Begin);
        }
        catch (Exception ex)
        {
            throw new Exception("Error while downloading file from GridFS: " + ex.Message);
        }

        return fileStream;
    }

    // Get file metadata
    public async Task<GridFSFileInfo> GetFileInfoAsync(ObjectId fileId)
    {
        try
        {
            var filter = Builders<GridFSFileInfo>.Filter.Eq("_id", fileId);
            var fileInfoCursor = await _gridFSBucket.FindAsync(filter);
            var fileInfo = await fileInfoCursor.FirstOrDefaultAsync();

            if (fileInfo == null)
            {
                throw new Exception("File not found in GridFS.");
            }

            return fileInfo;
        }
        catch (Exception ex)
        {
            throw new Exception("Error while fetching file metadata: " + ex.Message);
        }
    }
}
