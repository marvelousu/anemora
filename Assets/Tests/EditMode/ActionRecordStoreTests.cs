using System;
using System.Linq;
using NUnit.Framework;
using Anemora.Data;

namespace Anemora.Tests.EditMode
{
    [TestFixture]
    public class ActionRecordStoreTests
    {
        [Test]
        public void Add_AppendsEntry()
        {
            var store = new ActionRecordStore();
            store.Add(new ActionRecordEntry { actionId = "take_book_001" });

            Assert.AreEqual(1, store.Entries.Count);
            Assert.AreEqual("take_book_001", store.Entries[0].actionId);
        }

        [Test]
        public void Add_NullEntry_Throws()
        {
            var store = new ActionRecordStore();
            Assert.Throws<ArgumentNullException>(() => store.Add(null));
        }

        [Test]
        public void Add_EmptyActionId_Throws()
        {
            var store = new ActionRecordStore();
            Assert.Throws<ArgumentException>(() => store.Add(new ActionRecordEntry { actionId = "" }));
            Assert.Throws<ArgumentException>(() => store.Add(new ActionRecordEntry { actionId = null }));
        }

        [Test]
        public void GetUnreflected_ReturnsOnlyUnreflected()
        {
            var store = new ActionRecordStore();
            store.Add(new ActionRecordEntry { actionId = "a", reflected = true });
            store.Add(new ActionRecordEntry { actionId = "b", reflected = false });
            store.Add(new ActionRecordEntry { actionId = "c", reflected = false });

            var unreflected = store.GetUnreflected().ToList();

            Assert.AreEqual(2, unreflected.Count);
            CollectionAssert.AreEquivalent(new[] { "b", "c" }, unreflected.Select(e => e.actionId));
        }

        [Test]
        public void GetReflected_ReturnsOnlyReflected()
        {
            var store = new ActionRecordStore();
            store.Add(new ActionRecordEntry { actionId = "a", reflected = true });
            store.Add(new ActionRecordEntry { actionId = "b", reflected = false });

            var reflected = store.GetReflected().ToList();

            Assert.AreEqual(1, reflected.Count);
            Assert.AreEqual("a", reflected[0].actionId);
        }

        [Test]
        public void MarkReflected_SetsFlagAndReturnsTrue()
        {
            var store = new ActionRecordStore();
            store.Add(new ActionRecordEntry { actionId = "a", reflected = false });

            var result = store.MarkReflected("a");

            Assert.IsTrue(result);
            Assert.IsTrue(store.Entries[0].reflected);
        }

        [Test]
        public void MarkReflected_AlreadyReflected_ReturnsFalse()
        {
            var store = new ActionRecordStore();
            store.Add(new ActionRecordEntry { actionId = "a", reflected = true });

            var result = store.MarkReflected("a");

            Assert.IsFalse(result);
        }

        [Test]
        public void MarkReflected_UnknownId_ReturnsFalse()
        {
            var store = new ActionRecordStore();
            store.Add(new ActionRecordEntry { actionId = "a", reflected = false });

            var result = store.MarkReflected("unknown");

            Assert.IsFalse(result);
            Assert.IsFalse(store.Entries[0].reflected);
        }

        [Test]
        public void MarkReflected_PreventsDoubleReflection()
        {
            var store = new ActionRecordStore();
            store.Add(new ActionRecordEntry { actionId = "a", reflected = false });

            Assert.IsTrue(store.MarkReflected("a"));
            Assert.IsFalse(store.MarkReflected("a"));

            Assert.AreEqual(1, store.Entries.Count(e => e.reflected));
        }

        [Test]
        public void RoundTrip_PreservesEntries()
        {
            var original = new ActionRecordStore();
            original.Add(new ActionRecordEntry
            {
                actionId = "take_book_001",
                targetObjectId = "Book_Family_Past_001",
                type = ActionType.Take,
                gameTimeTicks = 12345,
                reflected = false
            });

            var saveData = original.ToSaveData();
            var restored = new ActionRecordStore();
            restored.LoadFromSaveData(saveData);

            Assert.AreEqual(1, restored.Entries.Count);
            Assert.AreEqual("take_book_001", restored.Entries[0].actionId);
            Assert.AreEqual("Book_Family_Past_001", restored.Entries[0].targetObjectId);
            Assert.AreEqual(ActionType.Take, restored.Entries[0].type);
            Assert.AreEqual(12345, restored.Entries[0].gameTimeTicks);
            Assert.IsFalse(restored.Entries[0].reflected);
        }

        [Test]
        public void RoundTrip_DoesNotShareReferences()
        {
            var original = new ActionRecordStore();
            original.Add(new ActionRecordEntry { actionId = "a", reflected = false });

            var saveData = original.ToSaveData();
            saveData.entries[0].reflected = true;

            Assert.IsFalse(original.Entries[0].reflected, "ToSaveData must produce a deep copy");
        }

        [Test]
        public void LoadFromSaveData_NullClearsStore()
        {
            var store = new ActionRecordStore();
            store.Add(new ActionRecordEntry { actionId = "a" });

            store.LoadFromSaveData(null);

            Assert.AreEqual(0, store.Entries.Count);
        }
    }
}
