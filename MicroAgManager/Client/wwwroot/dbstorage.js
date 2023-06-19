export function synchronizeFileWithIndexedDb(filename) {
    return new Promise((res, rej) => {
        const db = window.indexedDB.open('SqliteStorage', 1);
        db.onupgradeneeded = () => {
            db.result.createObjectStore('Files', { keypath: 'id' });
        };

        db.onsuccess = () => {
            const req = db.result.transaction('Files', 'readonly').objectStore('Files').get('file');
            req.onsuccess = () => {
                window.Module.FS_createDataFile('/', filename, req.result, true, true, true);
                res();
            };
        };

        let lastModifiedTime = new Date();
        setInterval(() => {
            const path = `/${filename}`;
            if (window.Module.FS.analyzePath(path).exists) {
                const mtime = window.Module.FS.stat(path).mtime;
                if (mtime.valueOf() !== lastModifiedTime.valueOf()) {
                    lastModifiedTime = mtime;
                    const data = window.Module.FS.readFile(path);
                    db.result.transaction('Files', 'readwrite').objectStore('Files').put(data, 'file');
                }
            }
        }, 1000);
    });
}
export async function saveToBrowserCache(filename) {
    const cache = await caches.open("BlazorDBCache");

    const binaryData = window.Module.FS.readFile(filename);

    const blob = new Blob([binaryData], {
        type: 'application/octet-stream',
        ok: true,
        status: 200
    });

    const headers = new Headers({
        'content-length': blob.size
    });

    const response = new Response(blob, {
        headers
    });

    await cache.put("BlazorDB", response);
}
export async function deleteDBFromCache() {
    const cache = await caches.open("BlazorDBCache");
    cache.delete("BlazorDB");
}
export async function generateDownloadLink() {
    const cache = await caches.open("BlazorDBCache");
    const resp = await cache.match("BlazorDB");

    if (resp && resp.ok) {

        const res = await resp.blob();
        if (res) {
            return URL.createObjectURL(res);
        }
    }

    return "";
}
window.bufferToCanvas = function(elem, buffer, width, height) {
    let imageData = new ImageData(new Uint8ClampedArray(buffer.buffer, 0, width * height * 4), width, height);
    elem.width = width;
    elem.height = height;
    elem.getContext('2d').putImageData(imageData, 0, 0);
}
