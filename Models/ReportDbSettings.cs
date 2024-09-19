

namespace RipStainAPI.Models;

public class ReportDbSettings {
    public string ConnectionString {get; set; } = null!;

    public string DatabaseName {get; set; } = null!;

    public string ReportsCollection {get; set; } = null!;
}
