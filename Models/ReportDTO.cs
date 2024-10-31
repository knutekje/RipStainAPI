using RipStainAPI;

public class ReportDTO (){
    public string JsonObject { get; set; }  // JSON string to be deserialized
    public IFormFile File { get; set; }     // File upload
}