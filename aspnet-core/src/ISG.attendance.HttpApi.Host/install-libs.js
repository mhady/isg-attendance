const fs = require('fs');
const path = require('path');

const baseDir = __dirname;
const nodeModulesDir = path.join(baseDir, 'node_modules');
const wwwrootLibsDir = path.join(baseDir, 'wwwroot', 'libs');

// Simple glob matcher for wildcards
function matchFiles(pattern, baseDir) {
    const parts = pattern.split('*');
    if (parts.length === 1) {
        // No wildcard, return as is if exists
        return fs.existsSync(path.join(baseDir, pattern)) ? [pattern] : [];
    }

    const dirPath = path.dirname(pattern);
    const fullDirPath = path.join(baseDir, dirPath);

    if (!fs.existsSync(fullDirPath)) {
        return [];
    }

    const files = fs.readdirSync(fullDirPath);
    const baseName = path.basename(pattern);

    // Simple wildcard matching
    const matched = [];
    files.forEach(file => {
        const fullPath = path.join(fullDirPath, file);
        const stats = fs.statSync(fullPath);
        if (stats.isFile()) {
            // Check if file matches pattern
            if (baseName === '*.*' || baseName === '*') {
                matched.push(path.join(dirPath, file));
            } else if (baseName.startsWith('*.')) {
                const ext = baseName.substring(1);
                if (file.endsWith(ext)) {
                    matched.push(path.join(dirPath, file));
                }
            }
        }
    });

    return matched;
}

// Find all abp.resourcemapping.js files
const abpDir = path.join(nodeModulesDir, '@abp');
const abpPackages = fs.readdirSync(abpDir);
const mappingFiles = [];

abpPackages.forEach(pkg => {
    const mappingFile = path.join(abpDir, pkg, 'abp.resourcemapping.js');
    if (fs.existsSync(mappingFile)) {
        mappingFiles.push(mappingFile);
    }
});

console.log(`Found ${mappingFiles.length} resource mapping files`);

// Create wwwroot/libs if it doesn't exist
if (!fs.existsSync(wwwrootLibsDir)) {
    fs.mkdirSync(wwwrootLibsDir, { recursive: true });
}

// Process each mapping file
mappingFiles.forEach(mappingFile => {
    console.log(`\nProcessing: ${path.basename(path.dirname(mappingFile))}`);

    const mapping = require(mappingFile);

    if (mapping && mapping.mappings) {
        Object.entries(mapping.mappings).forEach(([source, dest]) => {
            // Replace @node_modules with actual path
            const sourcePath = source.replace('@node_modules/', '');
            // Replace @libs with actual wwwroot/libs path
            const destPath = dest.replace('@libs/', '');

            // Handle wildcards
            if (sourcePath.includes('*')) {
                const files = matchFiles(sourcePath, nodeModulesDir);
                console.log(`  Found ${files.length} files matching ${sourcePath}`);

                files.forEach(file => {
                    const fileName = path.basename(file);
                    const fullDestPath = path.join(wwwrootLibsDir, destPath);
                    const fullDestFile = path.join(fullDestPath, fileName);

                    // Create destination directory if it doesn't exist
                    if (!fs.existsSync(fullDestPath)) {
                        fs.mkdirSync(fullDestPath, { recursive: true });
                    }

                    // Copy file
                    const fullSourceFile = path.join(nodeModulesDir, file);
                    fs.copyFileSync(fullSourceFile, fullDestFile);
                    console.log(`    Copied: ${fileName}`);
                });
            } else {
                // Direct file copy
                const fullSourcePath = path.join(nodeModulesDir, sourcePath);
                const fullDestPath = path.join(wwwrootLibsDir, destPath);
                const fileName = path.basename(sourcePath);
                const fullDestFile = path.join(fullDestPath, fileName);

                if (!fs.existsSync(fullDestPath)) {
                    fs.mkdirSync(fullDestPath, { recursive: true });
                }

                if (fs.existsSync(fullSourcePath)) {
                    fs.copyFileSync(fullSourcePath, fullDestFile);
                    console.log(`    Copied: ${fileName}`);
                }
            }
        });
    }
});

console.log('\nLibrary installation complete!');
