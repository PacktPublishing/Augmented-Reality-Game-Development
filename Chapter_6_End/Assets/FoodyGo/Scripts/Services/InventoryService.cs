using SQLite4Unity3d;
using UnityEngine;
using packt.FoodyGO.Database;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using packt.FoodyGO.Managers;
using System;

namespace packt.FoodyGO.Services
{
    public class InventoryService : Singleton<InventoryService>
    {
        public string DatabaseName = "foodygo.db";
        public string DatabaseVersion = "1.0.0";
        private bool newDatabase;
        private SQLiteConnection _connection;
        
        // Use this for initialization
        //use Awake instead of Start for earlier initialization
        void Awake()
        {
#if UNITY_EDITOR
        var dbPath = string.Format(@"Assets/StreamingAssets/{0}", DatabaseName);
        if (!File.Exists(dbPath))
        {
            newDatabase = true;
        }
#else
            // check if file exists in Application.persistentDataPath
            var filepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);

            if (!File.Exists(filepath))
            {
                Debug.Log("Database not in Persistent path");
                // if it doesn't ->
                // open StreamingAssets directory and load the db ->

#if UNITY_ANDROID
            var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName);  // this is the path to your StreamingAssets in android
            while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
                 var loadDb = Application.dataPath + "/Raw/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#elif UNITY_WP8
                var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);

#elif UNITY_WINRT
		var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
#else
                var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                                                                                         // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);

#endif

                Debug.Log("Database written");
                newDatabase = true;
            }

            var dbPath = filepath;
#endif
            _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
            Debug.Log("Final PATH: " + dbPath);

            if (newDatabase)
            {
                CreateDB();
            }else
            {
                CheckForUpgrade();
            }

        }

        private void CheckForUpgrade()
        {
            try
            {
                var version = GetDatabaseVersion();
                if (CheckDBVersion(version))
                {
                    //newer version upgrade required
                    Debug.LogFormat("Database current version {0} - upgrading to {1}", version, DatabaseVersion);
                    UpgradeDB();
                    Debug.Log("Database upgraded");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to upgrade database, running CreateDB instead");
                Debug.LogError("Error - " + ex.Message);
                CreateDB();
            }
        }

        private bool CheckDBVersion(string version)
        {
            var current = version.Split('.');
            var dbVersion = DatabaseVersion.Split('.');

            if (current.Length != dbVersion.Length)
                throw new ApplicationException("Database version numbers do not match.");
            for(int i = 0; i < current.Length; i++)
            {
                if (int.Parse(current[i]) < int.Parse(dbVersion[i])) return true;
            }
            return false;
        }

        private void CreateDB()
        {
            Debug.Log("Creating database...");
            var minfo = _connection.GetTableInfo("Monster");
            if(minfo.Count>0) _connection.DropTable<Monster>();                        
            _connection.CreateTable<Monster>();
            Debug.Log("Monster table created.");
            var vinfo = _connection.GetTableInfo("DatabaseVersion");
            if(vinfo.Count>0) _connection.DropTable<DatabaseVersion>();
            _connection.CreateTable<DatabaseVersion>();
            Debug.Log("DatabaseVersion table created.");

            _connection.Insert(new DatabaseVersion
            {
                Version = DatabaseVersion
            });
            Debug.Log("Database version updated to " + DatabaseVersion);
            Debug.Log("Database created.");
        }

        private void UpgradeDB()
        {
            var monsters = _connection.Table<Monster>();
            //need to manually convert this to a list
            //Linq is not supported on iOS
            List<Monster> list = new List<Monster>();
            foreach(var m in monsters)
            {
                list.Add(m);
            }
            CreateDB();
            Debug.Log("Replacing monsters.");
            _connection.InsertAll(list);
        }

        public string GetDatabaseVersion()
        {
            return _connection.Table<DatabaseVersion>().FirstOrDefault().Version;
        }

        public Monster CreateMonster(Monster m)
        {
            var id = _connection.Insert(m);
            m.Id = id;
            return m;
        }

        public Monster ReadMonster(int id)
        {
            return _connection.Table<Monster>()
                .Where(m => m.Id == id).FirstOrDefault();
        }

        public IEnumerable<Monster> ReadMonsters()
        {
            return _connection.Table<Monster>();
        }

        public int UpdateMonster(Monster m)
        {
            return _connection.Update(m);
        }

        public int DeleteMonster(Monster m)
        {
            return _connection.Delete(m);
        }

    }
}
