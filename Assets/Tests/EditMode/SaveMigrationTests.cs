using NUnit.Framework;
using Newtonsoft.Json.Linq;
using Anemora.Save.Migration;

namespace Anemora.Tests.EditMode
{
    [TestFixture]
    public class SaveMigrationTests
    {
        private sealed class V1ToV2RenameMigration : ISaveMigration
        {
            public int FromVersion => 1;
            public int ToVersion => 2;

            public JObject Migrate(JObject source)
            {
                if (source["oldField"] != null)
                {
                    source["newField"] = source["oldField"];
                    source.Remove("oldField");
                }
                source["saveVersion"] = ToVersion;
                return source;
            }
        }

        [Test]
        public void Migration_TransformsField()
        {
            var src = JObject.Parse("{\"saveVersion\":1,\"oldField\":\"value\"}");
            var migration = new V1ToV2RenameMigration();

            var result = migration.Migrate(src);

            Assert.IsNull(result["oldField"]);
            Assert.AreEqual("value", (string)result["newField"]);
            Assert.AreEqual(2, (int)result["saveVersion"]);
        }

        [Test]
        public void Migration_HasSequentialVersionStep()
        {
            var migration = new V1ToV2RenameMigration();

            Assert.AreEqual(1, migration.FromVersion);
            Assert.AreEqual(2, migration.ToVersion);
            Assert.AreEqual(migration.FromVersion + 1, migration.ToVersion,
                "ADR-0006 §7: SaveMigrator は fromVersion -> fromVersion + 1 の逐次移行のみ");
        }

        [Test]
        public void Migration_PreservesUnrelatedFields()
        {
            var src = JObject.Parse("{\"saveVersion\":1,\"oldField\":\"value\",\"otherField\":42}");
            var migration = new V1ToV2RenameMigration();

            var result = migration.Migrate(src);

            Assert.AreEqual(42, (int)result["otherField"]);
        }
    }
}
