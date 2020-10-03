# Contribution

You can contribute by

* Testing on your machine with any exotic hardware you have. Such devices might
  be an Iomega ZIP drive (if they still exist), or other actual SCSI devices.
* Reviewing the code and offering bugfixes.
* Updating the documentation within the code.

## Contributing Changes in Code

You can contribute a change by emailing a patch set to `jcurl@arcor.de`, or on
GitHub, by forking the code base, making your changes and referring me to those
changes (either as a commit identifier in an isue, or a pull request).

The changes will be reviewed, and if I have time, I could merge it into the code
base.

Please be sure, that all existing unit tests pass, and consider writing in
addition new unit tests that shows the fix to the change/bug.

### Using new P/Invokes

If you need to add functionality that queries the OS via P/Invoke calls, please
be sure to modify the code to log that data (see the `Log` namespace), so new
unit tests can be created from it. It speeds up development (so testing doesn't
need to be run manually on various machines, some of which might no longer be
available) and minimizes regressions.

## Testing on Exotic (or not so common) Hardware Media

If you have some external devices, that are not common, please contribute by
capturing logs for that data. To capture logs, you run the `VolumeInfo.exe -l
<path>`. You'll need to specify multiple paths so that these can be incorporated
into new unit tests.

For example, a USB reader, drive `H:`, would be run first as:

```cmd
VolumeInfo H:\
```

It will generate output including the Volume Device `\\?\Volume{xxxx}`. Then log
that information and post via email, or as an issue in GitHub, the XML file
generated. Replace the `xxxx` with the real GUID that is shown.

```cmd
VolumeInfo -l H: H:\ \\?\Volume{xxxx} \\?\Volume{xxx}\
```

Then review and post the XML file.

If the media supports removable, then remove the media, rerun the same test and
provide the results again.

Please indicate the OS that you're testing on, as the API results from Windows
changes over generations.
