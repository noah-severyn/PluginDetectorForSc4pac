# Plugin Detector For Sc4pac

A cross plaform companion program for [sc4pac](https://memo33.github.io/sc4pac/#/) ([STEX](https://community.simtropolis.com/files/file/36700-sc4pac-mod-manager/)), designed to help you compare your manually installed plugins to sc4pac plugins. 

One of the main issues users face when converting their old plugins folders over to using sc4pac to manage their plugins is the difficulty of knowing a) what they have that is available in sc4pac and b) whether they have accidentally installed any duplicates of the same content.

When moving plugins, all of your existing plugins should go into the `075-my-plugins` or `895-my-overrides` folders. For the purpose of discussion these will be called "manually installed" plugins. As you install plugins via sc4pac, you would want to manually delete the now redundant items from your `075` and `895` folders. This may not be simple if you are not intimately familiar with the contents your plugins folder, or if your plugins folder is particularly large. This tool is designed to assist with this process.


The goals for this tool are:
- Identify what manually installed plugins that duplicate what has been installed with sc4pac
- Facilitate easy removal of manually installed plugins, in favor of the sc4pac installed ones

A stretch goal:
- Determine which manually installed files are available in sc4pac for install

Follow the [Development Thread](https://community.simtropolis.com/forums/topic/764122-plugin-detector-for-sc4pac-development-thread/) at Simtropolis for updates and further instructions.
