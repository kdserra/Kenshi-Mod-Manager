# Kenshi Mod Manager
 
Tool to manage your mods for Kenshi.

## Attribution

See the [attribution](./ATTRIBUTION.md) file.

# Ideas
- Filters
    - Search Bar
    - Show only active mods
    - Show only inactive mods

# Todo
- `npm run package` button images css is broken
    (assets in wrong folder, cmd puts it in `resources\assets`,
    css var expects relative (`var(../../assets/icon.png)`)
    goes back 2 directories)
- switch to ts-node, for two reasons:
    1. no need to build during development
    2. no need for shared public html/css directory (which has also issues when packaging)
- fix HTML not resizing image (Khosphere mod), force resize to 40px by 40px

## License

Kenshi Mod Manager is [MIT licensed](./LICENSE).