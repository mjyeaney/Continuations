Continuations
=============

This is a little set of test cases around building a reusable continuation paradigm for code that must complete a set of
work in the face of failure. Similar to the [CommonJS][1] promise spec, the idea here is to remove superfluous error handling
from the code doing the actual work, allowing for greater readability and maintainability.

Only in the prototype phases, mainly just experimental. Tests included (relies on nUnit); build script available for Win
(working on Mac shell script).

[1]: http://commonjs.org
