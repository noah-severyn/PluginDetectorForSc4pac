using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using csDBPF;

namespace PluginDetectorForSc4pac.Services {
    internal class DbpfService : IDbpfService {



        //public void ParseFolder(string path) {
            //var files = Directory.EnumerateFiles(path);
            //List<ScanResult> results = [];
            //List<FoundTGI> myTgis = [];
            //List<FoundTGI> pacTGgis = [];

            //var myFiles = files.Where(p => p.Contains("5-my-"));
            //if (myFiles.Count() == 0) {
            //    //do something here
            //}

            //var pacFiles = files.Where(p => !p.Contains("5-my-"));
            

            //foreach (string file in files) {
            //    if (DBPFUtil.IsDBPF(file)) {
            //        DBPFFile dbpf = new DBPFFile(file);
            //        var tgis = dbpf.ListOfTGIs;
            //        foreach (TGI tgi in tgis) {
            //            if (file.Contains("5-my-")) {
            //                myTgis.Add(new FoundTGI(file, tgi));
            //            } else {
            //                pacTGgis.Add(new FoundTGI(file, tgi));
            //            }
            //        }
            //    }
            //}
            //myTgis.Sort();
            //pacTGgis.Sort();


            //// Find records in listA that have a matching TGI in listB
            //var matches2 = _myTgis.Where(a => _pacTGgis.Any(b => b.Tgi.Equals(a.Tgi))).ToList();

            ////Much faster for large lists (O(n) instead of O(n²)).
            ////Requires TGI to have correct equality semantics.
            //var pactgis = new HashSet<TGI>(_pacTGgis.Select(t => t.Tgi));
            //var matches = _myTgis.Where(m => pactgis.Contains(m.Tgi)).ToList();

            //var pairs = from m in myTgis
            //            join p in pacTGgis on m.Tgi equals p.Tgi
            //            select (m, p);
            //foreach (var (m, p) in pairs) {
            //    Debug.WriteLine($"Match: {m.File} and {p.File} share {m.Tgi}");
            //    results.Add(new ScanResult(m.File, p.File, m.Tgi.ToString()));
            //}
            //return results;
        //}

        public async Task<IEnumerable<ScanResult>> ScanAsync(string folder, IProgress<ScanProgress>? progress = null) {
            var files = Directory.EnumerateFiles(folder, "*", SearchOption.AllDirectories);
            List<ScanResult> results = [];
            List<FoundTGI> myTgis = [];
            List<FoundTGI> pacTGgis = [];

            var myFiles = files.Where(p => p.Contains("5-my-")).Select(f => new FoundFile(f, Path.GetFileName(f)));
            if (!myFiles.Any()) {
                //do something here
            }

            var pacFiles = files.Where(p => !p.Contains("5-my-")).Select(f => new FoundFile(f, Path.GetFileName(f)));
            
            //int idx = 1;
            //foreach (string file in files) {
            //    progress?.Report(new ScanProgress(idx, files.Count()));
            //    if (DBPFUtil.IsDBPF(file)) {
            //        DBPFFile dbpf = new DBPFFile(file);
            //        var tgis = dbpf.ListOfTGIs;
            //        foreach (TGI tgi in tgis) {
            //            if (file.Contains("5-my-")) {
            //                myTgis.Add(new FoundTGI(file, Path.GetFileName(file), tgi));
            //            } else {
            //                pacTGgis.Add(new FoundTGI(file, Path.GetFileName(file), tgi));
            //            }
            //        }
            //    }
            //    idx++;
            //}
            //myTgis.Sort();
            //pacTGgis.Sort();

            //var pairs = from m in myTgis
            //            join p in pacTGgis on m.Tgi equals p.Tgi
            //            select (m, p);
            //foreach (var (m, p) in pairs) {
            //    Debug.WriteLine($"Match: {m.FileName} and {p.FileName} share {m.Tgi}");
            //    results.Add(new ScanResult(m.FileName, p.FileName, m.Tgi.ToString()));
            //}

            var pairs = from m in myFiles
                        join p in pacFiles on m.Filename equals p.Filename
                        select (m, p);
            foreach ((FoundFile m, FoundFile p) in pairs) {
                results.Add(new ScanResult(m.Path, m.Filename, ExtractSc4pacPackage(p.Path)));
            }

            return results;

            //for (int i = 0; i < files.Length; i++) {
            //    // simulate scanning
            //    await Task.Delay(50);

            //    results.Add(new ScanResult("tgi", files[i], "nonpac"));

            //    progress?.Report(new ScanProgress(i + 1, files.Length));
            //}
        }

        private static string ExtractSc4pacPackage(string path) {
            //Example paths are:
            //"C:\Users\Administrator\Documents\SimCity 4\Plugins\100-props-textures\simmer2.mega-prop-pack-vol1.1.0.sc4pac\SM2 Mega Prop Pack Vol1.dat"
            //"C:\Users\Administrator\Documents\SimCity 4\Plugins\300-commercial\b62.safeway-60s-retro-grocery.1.sc4pac\B62-Safeway 60's Retro\b62 safeway_sign_70s v 2.0-0x6534284a-0x50ec22ad-0xf2a67ca6.SC4Desc"
            string? pkg = path.Split('\\').FirstOrDefault(f => f.Contains(".sc4pac"));
            var pkgParts = pkg?.Split('.').SkipLast(2).ToArray(); //remove the version and sc4pac suffex
            return pkgParts?[0] + ":" + pkgParts?[1];
        }

        private record struct FoundFile(string Path, string Filename);

        private record struct FoundTGI(string Path, string FileName, TGI Tgi) : IComparable<FoundTGI> {


            /// <inheritdoc/>
            public readonly int CompareTo(FoundTGI other) {
                if (Path != other.Path) {
                    return Path.CompareTo(other.Path);
                }
                return Tgi.CompareTo(other.Tgi);
            }
        }
    }
}
