mergeInto(LibraryManager.library, {
  SyncFiles: function () {
    FS.syncfs(false, function (err) {
      if (err) {
        console.error("SyncFiles error:", err);
      } else {
        console.log("SyncFiles: save data flushed to IndexedDB.");
      }
    });
  }
});