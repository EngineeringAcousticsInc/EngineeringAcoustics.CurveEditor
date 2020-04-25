# EngineeringAcoustics.CurveEditor
A fork of the XNA Curve Editor tool that uses the bare minimum of FNA to not depend on the XNA Framework install.

## Differences
For now, `EngineeringAcoustics.Curve` includes some other curve tools/features.
 * `ICurve` - An interface that describes a curve.
 * `SimpleCurve` - Just a wrapper for the XNA Curve but implements `ICurve`.
 * `ComplexCurve` - An `ICurve` that has an underlying `ICurve` and supports mutators.
 * `IComplexCurveMutator` - An interface that describes a mutator for `ComplexCurve` to transform curve values.
   * `ComplexCurvePassthruMutator` - A bare `IComplexCurveMutator` implementation that does not modify the value at all.
   * `ComplexCurveMutator` - A base `IComplexCurveMutator` implementation, that supports a single generator, and one operation (multiply/add/subtract).
 * `IFunctionGenerator` - An interface that describes a function generator that returns a value based on an input.
   * `FunctionConstant` - A function generator that returns a fixed value regardless of requested position.
   * No other functions are implemented yet, but planned are: Sine, Triangle, Sawtooth (& inverse), Square, Random, and perhaps more (FFT?)...

## Future
Ideally the curve editor should hopefully be replaced with a custom implementation suitable for use in projects that don't have any interest in XNA.

A replacement editor will undoubtedly be more simple (to fit our needs), but should hopefully be easier to use, but be able to produce great curves.

As well, the XNA Curve (via FNA) currently uses the following methods to generate values:
 * Immediate stepping
 * Linear interpolation (when using 'Linear' tangents)
 * A [cubic Hermite interpolation](https://en.wikipedia.org/wiki/Cubic_Hermite_spline#Representations) (when using 'Smooth' or 'Flat' tangents)

I'm hoping to add other interpolations, as well as expose more of the knobs to tweak on the generation so curves can look just like you want them to.
Perhaps algorithms such as B-Spline (de Boor), Bezier via Bernstein/DeCasteljau/Midpoint subdivision, etc.

## Not Implemented
Save/Load is not implemented and will throw a NYI if called.

## Thanks
FNA: https://fna-xna.github.io/ (Ms-PL, Code from Mono.Xna under MIT)
XNA Curve Editor Tool: Via the XNAGameStudio library, https://github.com/SimonDarksideJ/XNAGameStudio/wiki/Curve-Editor (Ms-PL)