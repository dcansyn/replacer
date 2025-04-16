using System.Text;

string[] replaceExcludes =
    "png,jpeg,jpg,bmp,gif,pdf,doc,docx,xlsx,xls,mp4,mp3,wav,wma,aac,flac,m4a,woff,woff2,tiff,svg"
    .Split(',')
    .Select(x => $".{x}")
    .ToArray();

Console.WriteLine("Değiştirme sırasında mevcut dosyalar korunsun mu? y/n");
var skipExistsFile = Console.ReadLine() == "y";

Console.WriteLine("Varsayılan kayıdı atlamak ister misin? y/n");
var skillDefaultReplace = Console.ReadLine() == "y";

Console.WriteLine("Lütfen klasör yolunu giriniz.");
var currentDomainDirectory = new DirectoryInfo(Console.ReadLine() ?? string.Empty);
if (currentDomainDirectory == null || !currentDomainDirectory.Exists) throw new Exception("Klasör bulunamadı.");

Console.WriteLine("Lütfen yeni klasör adını giriniz.");
var newDomainDirectory = new DirectoryInfo(Console.ReadLine() ?? string.Empty);
if (newDomainDirectory == null || newDomainDirectory.Exists) throw new Exception("Klasör zaten var.");

Console.WriteLine("Lütfen değiştirmek istediğiniz isimleri giriniz.");
Console.WriteLine("Örneğin: Domain:Application");

var input = string.Empty;
var replaceData = new Dictionary<string, string>();

if (!skillDefaultReplace) replaceData.Add(currentDomainDirectory.Name, newDomainDirectory.Name);

do
{
    input = Console.ReadLine();
    if (input != null && input.Contains(':'))
    {
        var first = input.Split(':').FirstOrDefault();
        var last = input.Split(':').LastOrDefault();
        if (first == null || last == null)
        {
            Console.WriteLine("Lütfen geçerli bir değer giriniz.");
            continue;
        }

        replaceData.TryAdd(first, last);
        replaceData.TryAdd(first.ToLower(), last.ToLower());
    }
} while (input != "ok");

if (currentDomainDirectory.Parent == null) throw new Exception("Üst kategori bulunamadı.");
var parentPath = currentDomainDirectory.Parent.FullName;

Move(currentDomainDirectory.Name);

void Move(string directoryPath)
{
    var path = Path.Combine(parentPath, directoryPath);

    var directories = Directory.GetDirectories(path);
    foreach (var directory in directories)
    {
        var directoryInfo = new DirectoryInfo(directory);
        Move($@"{directoryPath}\{directoryInfo.Name}");
    }

    var files = Directory.GetFiles(path);
    foreach (var item in files)
    {
        var fileInfo = new FileInfo(item);
        var destinationPath = $@"{directoryPath}\{fileInfo.Name}";

        foreach (var data in replaceData)
            destinationPath = destinationPath.Replace(data.Key, data.Value);

        var destinationFile = new FileInfo(Path.Combine(parentPath, destinationPath));
        if (destinationFile.DirectoryName != null && !Directory.Exists(destinationFile.DirectoryName))
            Directory.CreateDirectory(destinationFile.DirectoryName);

        if (skipExistsFile && File.Exists(destinationFile.FullName))
            continue;

        if (replaceExcludes.Contains(fileInfo.Extension))
        {
            File.Copy(fileInfo.FullName, destinationFile.FullName);
            continue;
        }

        using var sr = new StreamReader(fileInfo.FullName, new UTF8Encoding(false));
        using var st = new StreamWriter(destinationFile.FullName, false, new UTF8Encoding(false));

        var detail = sr.ReadToEnd();
        foreach (var data in replaceData)
            detail = detail.Replace(data.Key, data.Value);

        st.Write(detail);
    }
}
