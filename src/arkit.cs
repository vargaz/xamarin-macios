//
// ARKit bindings
//
// Authors:
//	Vincent Dondain  <vidondai@microsoft.com>
//
// Copyright 2017 Microsoft Inc. All rights reserved.
//

#if XAMCORE_2_0

using System;
using System.ComponentModel;
using AVFoundation;
using CoreFoundation;
using CoreGraphics;
using CoreMedia;
using CoreVideo;
using Foundation;
using ObjCRuntime;
using Metal;
using SpriteKit;
using SceneKit;
using UIKit;

using Vector2 = global::OpenTK.Vector2;
using Vector3 = global::OpenTK.NVector3;
using Matrix3 = global::OpenTK.NMatrix3;
using Matrix4 = global::OpenTK.NMatrix4;

namespace ARKit {

	[iOS (11,0)]
	[NoWatch, NoTV, NoMac]
	[Native]
	public enum ARTrackingState : long {
		NotAvailable,
		Limited,
		Normal,
	}

	[iOS (11,0)]
	[NoWatch, NoTV, NoMac]
	[Native]
	public enum ARTrackingStateReason : long {
		None,
		Initializing,
		ExcessiveMotion,
		InsufficientFeatures,
	}

	[iOS (11,0)]
	[NoWatch, NoTV, NoMac]
	[ErrorDomain ("ARErrorDomain")]
	[Native]
	public enum ARErrorCode : long {
		UnsupportedConfiguration = 100,
		SensorUnavailable = 101,
		SensorFailed = 102,
		CameraUnauthorized = 103,
		WorldTrackingFailed = 200,
	}

	[iOS (11,0)]
	[NoWatch, NoTV, NoMac]
	[Flags]
	[Native]
	public enum ARHitTestResultType : ulong {
		FeaturePoint = 1 << 0,
		EstimatedHorizontalPlane = 1 << 1,
		ExistingPlane = 1 << 3,
		ExistingPlaneUsingExtent = 1 << 4,
	}

	[iOS (11,0)]
	[NoWatch, NoTV, NoMac]
	[Native]
	public enum ARPlaneAnchorAlignment : long {
		Horizontal,
	}

	[iOS (11,0)]
	[NoWatch, NoTV, NoMac]
	[Flags]
	[Native]
	public enum ARSessionRunOptions : ulong {
		ResetTracking = 1 << 0,
		RemoveExistingAnchors = 1 << 1,
	}

	[iOS (11,0)]
	[NoWatch, NoTV, NoMac]
	[Native]
	public enum ARWorldAlignment : long {
		Gravity,
		GravityAndHeading,
		Camera,
	}

	[iOS (11,0)]
	[NoWatch, NoTV, NoMac]
	[Flags]
	[Native]
	public enum ARPlaneDetection : ulong {
		None = 0,
		Horizontal = 1 << 0,
	}

	[iOS (11,0)]
	[NoWatch, NoTV, NoMac]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface ARAnchor : NSCopying {

		[NullAllowed, Export ("identifier")]
		NSUuid Identifier { get; }

		[Export ("transform")]
		Matrix4 Transform {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}

		[Export ("initWithTransform:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		IntPtr Constructor (Matrix4 transform);
	}

	[iOS (11,0)]
	[NoWatch, NoTV, NoMac]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface ARCamera : NSCopying {

		[Export ("transform")]
		Matrix4 Transform {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}

		[Export ("eulerAngles")]
		Vector3 EulerAngles {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}

		[Export ("trackingState")]
		ARTrackingState TrackingState { get; }

		[Export ("trackingStateReason")]
		ARTrackingStateReason TrackingStateReason { get; }

		[Export ("intrinsics")]
		Matrix3 Intrinsics {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}

		[Export ("imageResolution")]
		CGSize ImageResolution { get; }

		[Export ("projectionMatrix")]
		Matrix4 ProjectionMatrix {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}

		[Export ("projectPoint:orientation:viewportSize:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		CGPoint GetProjectPoint (Vector3 point, UIInterfaceOrientation orientation, CGSize viewportSize);

		[Export ("projectionMatrixForOrientation:viewportSize:zNear:zFar:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		Matrix4 GetProjectionMatrix (UIInterfaceOrientation orientation, CGSize viewportSize, nfloat zNear, nfloat zFar);

		[Export ("viewMatrixForOrientation:")]
		[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
		Matrix4 GetViewMatrix (UIInterfaceOrientation orientation);
	}

	[iOS (11,0)]
	[NoWatch, NoTV, NoMac]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface ARFrame : NSCopying {

		[Export ("timestamp")]
		double Timestamp { get; }

		[Export ("capturedImage")]
		CVPixelBuffer CapturedImage { get; }

		[NullAllowed, Export ("capturedDepthData", ArgumentSemantic.Strong)]
		AVDepthData CapturedDepthData { get; }

		[Export ("capturedDepthDataTimestamp")]
		double CapturedDepthDataTimestamp { get; }

		[Export ("camera", ArgumentSemantic.Copy)]
		ARCamera Camera { get; }

		[Export ("anchors", ArgumentSemantic.Copy)]
		ARAnchor[] Anchors { get; }

		[NullAllowed, Export ("lightEstimate", ArgumentSemantic.Strong)]
		ARLightEstimate LightEstimate { get; }

		[NullAllowed, Export ("rawFeaturePoints", ArgumentSemantic.Strong)]
		ARPointCloud RawFeaturePoints { get; }

		[Export ("hitTest:types:")]
		ARHitTestResult[] HitTest (CGPoint point, ARHitTestResultType types);

		[Export ("displayTransformForOrientation:viewportSize:")]
		CGAffineTransform GetDisplayTransform (UIInterfaceOrientation orientation, CGSize viewportSize);
	}

	[iOS (11,0)]
	[NoWatch, NoTV, NoMac]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface ARHitTestResult {

		[Export ("type")]
		ARHitTestResultType Type { get; }

		[Export ("distance")]
		nfloat Distance { get; }

		[Export ("localTransform")]
		Matrix4 LocalTransform {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}

		[Export ("worldTransform")]
		Matrix4 WorldTransform {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}

		[NullAllowed, Export ("anchor", ArgumentSemantic.Strong)]
		ARAnchor Anchor { get; }
	}

	[iOS (11,0)]
	[NoWatch, NoTV, NoMac]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface ARLightEstimate {

		[Export ("ambientIntensity")]
		nfloat AmbientIntensity { get; }

		[Export ("ambientColorTemperature")]
		nfloat AmbientColorTemperature { get; }
	}

	[iOS (11,0)]
	[NoWatch, NoTV, NoMac]
	[BaseType (typeof (ARAnchor))]
	[DisableDefaultCtor]
	interface ARPlaneAnchor {

		// [Export ("initWithTransform:")] marked as NS_UNAVAILABLE

		[Export ("alignment")]
		ARPlaneAnchorAlignment Alignment { get; }

		[Export ("center")]
		Vector3 Center {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}

		[Export ("extent")]
		Vector3 Extent {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}
	}

	[iOS (11,0)]
	[NoWatch, NoTV, NoMac]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface ARPointCloud {

		[Export ("count")]
		nuint Count { get; }

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Protected, Export ("points")]
		IntPtr GetRawPoints ();

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Protected, Export ("identifiers")]
		IntPtr GetRawIdentifiers ();
	}

	[iOS (11,0)]
	[NoWatch, NoTV, NoMac]
	[BaseType (typeof (SCNView))]
	interface ARSCNView {

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		IARSCNViewDelegate Delegate { get; set; }

		[Export ("session", ArgumentSemantic.Strong)]
		ARSession Session { get; set; }

		[Export ("scene", ArgumentSemantic.Strong)]
		SCNScene Scene { get; set; }

		[Export ("automaticallyUpdatesLighting")]
		bool AutomaticallyUpdatesLighting { get; set; }

		[Export ("anchorForNode:")]
		[return: NullAllowed]
		ARAnchor GetAnchor (SCNNode node);

		[Export ("nodeForAnchor:")]
		[return: NullAllowed]
		SCNNode GetNode (ARAnchor anchor);

		[Export ("hitTest:types:")]
		ARHitTestResult[] HitTest (CGPoint point, ARHitTestResultType types);
	}

	interface IARSCNViewDelegate {}

	[iOS (11,0)]
	[NoWatch, NoTV, NoMac]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface ARSCNViewDelegate : SCNSceneRendererDelegate, ARSessionObserver {

		[Export ("renderer:nodeForAnchor:")]
		[return: NullAllowed]
		SCNNode GetNode (ISCNSceneRenderer renderer, ARAnchor anchor);

		[Export ("renderer:didAddNode:forAnchor:")]
		void DidAddNode (ISCNSceneRenderer renderer, SCNNode node, ARAnchor anchor);

		[Export ("renderer:willUpdateNode:forAnchor:")]
		void WillUpdateNode (ISCNSceneRenderer renderer, SCNNode node, ARAnchor anchor);

		[Export ("renderer:didUpdateNode:forAnchor:")]
		void DidUpdateNode (ISCNSceneRenderer renderer, SCNNode node, ARAnchor anchor);

		[Export ("renderer:didRemoveNode:forAnchor:")]
		void DidRemoveNode (ISCNSceneRenderer renderer, SCNNode node, ARAnchor anchor);
	}

	[iOS (11,0)]
	[NoWatch, NoTV, NoMac]
	[BaseType (typeof (SKView))]
	interface ARSKView {

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		IARSKViewDelegate Delegate { get; set; }

		[Export ("session", ArgumentSemantic.Strong)]
		ARSession Session { get; set; }

		[Export ("anchorForNode:")]
		[return: NullAllowed]
		ARAnchor GetAnchor (SKNode node);

		[Export ("nodeForAnchor:")]
		[return: NullAllowed]
		SKNode GetNode (ARAnchor anchor);

		[Export ("hitTest:types:")]
		ARHitTestResult[] HitTest (CGPoint point, ARHitTestResultType types);
	}

	interface IARSKViewDelegate {}

	[iOS (11,0)]
	[NoWatch, NoTV, NoMac]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface ARSKViewDelegate : SKViewDelegate, ARSessionObserver {

		[Export ("view:nodeForAnchor:")]
		[return: NullAllowed]
		SKNode GetNode (ARSKView view, ARAnchor anchor);

		[Export ("view:didAddNode:forAnchor:")]
		void DidAddNode (ARSKView view, SKNode node, ARAnchor anchor);

		[Export ("view:willUpdateNode:forAnchor:")]
		void WillUpdateNode (ARSKView view, SKNode node, ARAnchor anchor);

		[Export ("view:didUpdateNode:forAnchor:")]
		void DidUpdateNode (ARSKView view, SKNode node, ARAnchor anchor);

		[Export ("view:didRemoveNode:forAnchor:")]
		void DidRemoveNode (ARSKView view, SKNode node, ARAnchor anchor);
	}

	[iOS (11,0)]
	[NoWatch, NoTV, NoMac]
	[BaseType (typeof (NSObject))]
	interface ARSession {

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		IARSessionDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegateQueue", ArgumentSemantic.Strong)]
		DispatchQueue DelegateQueue { get; set; }

		[NullAllowed, Export ("currentFrame", ArgumentSemantic.Copy)]
		ARFrame CurrentFrame { get; }

		[NullAllowed, Export ("configuration", ArgumentSemantic.Copy)]
		ARConfiguration Configuration { get; }

		// 'runWithConfiguration:' selector marked as unavailable in Xcode 9 beta 5. Use 'Run (ARConfiguration configuration, ARSessionRunOptions options)' instead.
		[Export ("runWithConfiguration:options:")]
		void Run (ARConfiguration configuration, ARSessionRunOptions options);

		[Export ("pause")]
		void Pause ();

		[Export ("addAnchor:")]
		void AddAnchor (ARAnchor anchor);

		[Export ("removeAnchor:")]
		void RemoveAnchor (ARAnchor anchor);
	}

	[iOS (11,0)]
	[NoWatch, NoTV, NoMac]
	[Protocol]
	interface ARSessionObserver {

		[Export ("session:didFailWithError:")]
		void DidFail (ARSession session, NSError error);

		[Export ("session:cameraDidChangeTrackingState:")]
		void CameraDidChangeTrackingState (ARSession session, ARCamera camera);

		[Export ("sessionWasInterrupted:")]
		void WasInterrupted (ARSession session);

		[Export ("sessionInterruptionEnded:")]
		void InterruptionEnded (ARSession session);

		[Export ("session:didOutputAudioSampleBuffer:")]
		void DidOutputAudioSampleBuffer (ARSession session, CMSampleBuffer audioSampleBuffer);
	}

	interface IARSessionDelegate {}

	[iOS (11,0)]
	[NoWatch, NoTV, NoMac]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface ARSessionDelegate : ARSessionObserver {

		[Export ("session:didUpdateFrame:")]
		void DidUpdateFrame (ARSession session, ARFrame frame);

		[Export ("session:didAddAnchors:")]
		void DidAddAnchors (ARSession session, ARAnchor[] anchors);

		[Export ("session:didUpdateAnchors:")]
		void DidUpdateAnchors (ARSession session, ARAnchor[] anchors);

		[Export ("session:didRemoveAnchors:")]
		void DidRemoveAnchors (ARSession session, ARAnchor[] anchors);
	}

	[iOS (11,0)]
	[NoWatch, NoTV, NoMac]
	[BaseType (typeof (NSObject))]
	[Abstract]
	[DisableDefaultCtor]
	interface ARConfiguration : NSCopying {

		[Static]
		[Export ("isSupported")]
		bool IsSupported { get; }

		[Export ("worldAlignment", ArgumentSemantic.Assign)]
		ARWorldAlignment WorldAlignment { get; set; }

		[Export ("lightEstimationEnabled")]
		bool LightEstimationEnabled { [Bind ("isLightEstimationEnabled")] get; set; }

		[Export ("providesAudioData")]
		bool ProvidesAudioData { get; set; }
	}

	[iOS (11,0)]
	[NoWatch, NoTV, NoMac]
	[BaseType (typeof (ARConfiguration))]
	interface ARWorldTrackingConfiguration {

		[Export ("planeDetection", ArgumentSemantic.Assign)]
		ARPlaneDetection PlaneDetection { get; set; }
	}

	[iOS (11,0)]
	[NoWatch, NoTV, NoMac]
	[BaseType (typeof(ARConfiguration))]
	interface AROrientationTrackingConfiguration {}

	[iOS (11,0)]
	[NoWatch, NoTV, NoMac]
	[Static]
	interface ARSCNDebugOptions {

		[Field ("ARSCNDebugOptionShowWorldOrigin")]
		SCNDebugOptions ShowWorldOrigin { get; }

		[Field ("ARSCNDebugOptionShowFeaturePoints")]
		SCNDebugOptions ShowFeaturePoints { get; }
	}

	[iOS (11,0)]
	[NoWatch, NoTV, NoMac]
	[Protocol]
	interface ARTrackable {
		[Abstract]
		[Export ("isTracked")]
		bool IsTracked { get; }
	}

	[iOS (11,0)]
	[NoWatch, NoTV, NoMac]
	[BaseType (typeof(ARConfiguration))]
	interface ARFaceTrackingConfiguration {}

	[iOS (11,0)]
	[NoWatch, NoTV, NoMac]
	[StrongDictionary ("ARBlendShapeLocationKeys")]
	interface ARBlendShapeLocationOptions {

		float BrowDownLeft { get; set; }

		float BrowDownRight { get; set; }

		float BrowInnerUp { get; set; }

		float BrowOuterUpLeft { get; set; }

		float BrowOuterUpRight { get; set; }

		float CheekPuff { get; set; }

		float CheekSquintLeft { get; set; }

		float CheekSquintRight { get; set; }

		float EyeBlinkLeft { get; set; }

		float EyeBlinkRight { get; set; }

		float EyeLookDownLeft { get; set; }

		float EyeLookDownRight { get; set; }

		float EyeLookInLeft { get; set; }

		float EyeLookInRight { get; set; }

		float EyeLookOutLeft { get; set; }

		float EyeLookOutRight { get; set; }

		float EyeLookUpLeft { get; set; }

		float EyeLookUpRight { get; set; }

		float EyeSquintLeft { get; set; }

		float EyeSquintRight { get; set; }

		float EyeWideLeft { get; set; }

		float EyeWideRight { get; set; }

		float JawForward { get; set; }

		float JawLeft { get; set; }

		float JawOpen { get; set; }

		float JawRight { get; set; }

		float MouthClose { get; set; }

		float MouthDimpleLeft { get; set; }

		float MouthDimpleRight { get; set; }

		float MouthFrownLeft { get; set; }

		float MouthFrownRight { get; set; }

		float MouthFunnel { get; set; }

		float MouthLeft { get; set; }

		float MouthLowerDownLeft { get; set; }

		float MouthLowerDownRight { get; set; }

		float MouthPressLeft { get; set; }

		float MouthPressRight { get; set; }

		float MouthPucker { get; set; }

		float MouthRight { get; set; }

		float MouthRollLower { get; set; }

		float MouthRollUpper { get; set; }

		float MouthShrugLower { get; set; }

		float MouthShrugUpper { get; set; }

		float MouthSmileLeft { get; set; }

		float MouthSmileRight { get; set; }

		float MouthStretchLeft { get; set; }

		float MouthStretchRight { get; set; }

		float MouthUpperUpLeft { get; set; }

		float MouthUpperUpRight { get; set; }

		float NoseSneerLeft { get; set; }

		float NoseSneerRight { get; set; }
	}

	[iOS (11,0)]
	[NoWatch, NoTV, NoMac]
	[Static]
	[Internal]
	interface ARBlendShapeLocationKeys {

		[Field ("ARBlendShapeLocationBrowDownLeft")]
		NSString BrowDownLeftKey { get; }

		[Field ("ARBlendShapeLocationBrowDownRight")]
		NSString BrowDownRightKey { get; }

		[Field ("ARBlendShapeLocationBrowInnerUp")]
		NSString BrowInnerUpKey { get; }

		[Field ("ARBlendShapeLocationBrowOuterUpLeft")]
		NSString BrowOuterUpLeftKey { get; }

		[Field ("ARBlendShapeLocationBrowOuterUpRight")]
		NSString BrowOuterUpRightKey { get; }

		[Field ("ARBlendShapeLocationCheekPuff")]
		NSString CheekPuffKey { get; }

		[Field ("ARBlendShapeLocationCheekSquintLeft")]
		NSString CheekSquintLeftKey { get; }

		[Field ("ARBlendShapeLocationCheekSquintRight")]
		NSString CheekSquintRightKey { get; }

		[Field ("ARBlendShapeLocationEyeBlinkLeft")]
		NSString EyeBlinkLeftKey { get; }

		[Field ("ARBlendShapeLocationEyeBlinkRight")]
		NSString EyeBlinkRightKey { get; }

		[Field ("ARBlendShapeLocationEyeLookDownLeft")]
		NSString EyeLookDownLeftKey { get; }

		[Field ("ARBlendShapeLocationEyeLookDownRight")]
		NSString EyeLookDownRightKey { get; }

		[Field ("ARBlendShapeLocationEyeLookInLeft")]
		NSString EyeLookInLeftKey { get; }

		[Field ("ARBlendShapeLocationEyeLookInRight")]
		NSString EyeLookInRightKey { get; }

		[Field ("ARBlendShapeLocationEyeLookOutLeft")]
		NSString EyeLookOutLeftKey { get; }

		[Field ("ARBlendShapeLocationEyeLookOutRight")]
		NSString EyeLookOutRightKey { get; }

		[Field ("ARBlendShapeLocationEyeLookUpLeft")]
		NSString EyeLookUpLeftKey { get; }

		[Field ("ARBlendShapeLocationEyeLookUpRight")]
		NSString EyeLookUpRightKey { get; }

		[Field ("ARBlendShapeLocationEyeSquintLeft")]
		NSString EyeSquintLeftKey { get; }

		[Field ("ARBlendShapeLocationEyeSquintRight")]
		NSString EyeSquintRightKey { get; }

		[Field ("ARBlendShapeLocationEyeWideLeft")]
		NSString EyeWideLeftKey { get; }

		[Field ("ARBlendShapeLocationEyeWideRight")]
		NSString EyeWideRightKey { get; }

		[Field ("ARBlendShapeLocationJawForward")]
		NSString JawForwardKey { get; }

		[Field ("ARBlendShapeLocationJawLeft")]
		NSString JawLeftKey { get; }

		[Field ("ARBlendShapeLocationJawOpen")]
		NSString JawOpenKey { get; }

		[Field ("ARBlendShapeLocationJawRight")]
		NSString JawRightKey { get; }

		[Field ("ARBlendShapeLocationMouthClose")]
		NSString MouthCloseKey { get; }

		[Field ("ARBlendShapeLocationMouthDimpleLeft")]
		NSString MouthDimpleLeftKey { get; }

		[Field ("ARBlendShapeLocationMouthDimpleRight")]
		NSString MouthDimpleRightKey { get; }

		[Field ("ARBlendShapeLocationMouthFrownLeft")]
		NSString MouthFrownLeftKey { get; }

		[Field ("ARBlendShapeLocationMouthFrownRight")]
		NSString MouthFrownRightKey { get; }

		[Field ("ARBlendShapeLocationMouthFunnel")]
		NSString MouthFunnelKey { get; }

		[Field ("ARBlendShapeLocationMouthLeft")]
		NSString MouthLeftKey { get; }

		[Field ("ARBlendShapeLocationMouthLowerDownLeft")]
		NSString MouthLowerDownLeftKey { get; }

		[Field ("ARBlendShapeLocationMouthLowerDownRight")]
		NSString MouthLowerDownRightKey { get; }

		[Field ("ARBlendShapeLocationMouthPressLeft")]
		NSString MouthPressLeftKey { get; }

		[Field ("ARBlendShapeLocationMouthPressRight")]
		NSString MouthPressRightKey { get; }

		[Field ("ARBlendShapeLocationMouthPucker")]
		NSString MouthPuckerKey { get; }

		[Field ("ARBlendShapeLocationMouthRight")]
		NSString MouthRightKey { get; }

		[Field ("ARBlendShapeLocationMouthRollLower")]
		NSString MouthRollLowerKey { get; }

		[Field ("ARBlendShapeLocationMouthRollUpper")]
		NSString MouthRollUpperKey { get; }

		[Field ("ARBlendShapeLocationMouthShrugLower")]
		NSString MouthShrugLowerKey { get; }

		[Field ("ARBlendShapeLocationMouthShrugUpper")]
		NSString MouthShrugUpperKey { get; }

		[Field ("ARBlendShapeLocationMouthSmileLeft")]
		NSString MouthSmileLeftKey { get; }

		[Field ("ARBlendShapeLocationMouthSmileRight")]
		NSString MouthSmileRightKey { get; }

		[Field ("ARBlendShapeLocationMouthStretchLeft")]
		NSString MouthStretchLeftKey { get; }

		[Field ("ARBlendShapeLocationMouthStretchRight")]
		NSString MouthStretchRightKey { get; }

		[Field ("ARBlendShapeLocationMouthUpperUpLeft")]
		NSString MouthUpperUpLeftKey { get; }

		[Field ("ARBlendShapeLocationMouthUpperUpRight")]
		NSString MouthUpperUpRightKey { get; }

		[Field ("ARBlendShapeLocationNoseSneerLeft")]
		NSString NoseSneerLeftKey { get; }

		[Field ("ARBlendShapeLocationNoseSneerRight")]
		NSString NoseSneerRightKey { get; }
	}

	[iOS (11,0)]
	[NoWatch, NoTV, NoMac]
	[BaseType (typeof(ARAnchor))]
	interface ARFaceAnchor : ARTrackable {
		[Export ("geometry")]
		ARFaceGeometry Geometry { get; }

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("blendShapes")]
		NSDictionary WeakBlendShapes { get; }

		[Wrap ("WeakBlendShapes")]
		ARBlendShapeLocationOptions BlendShapes { get; }
	}

	[iOS (11,0)]
	[NoWatch, NoTV, NoMac]
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface ARFaceGeometry : NSCopying {
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("initWithBlendShapes:")]
		IntPtr Constructor (NSDictionary blendShapes);

		[Wrap ("this ((NSDictionary)blendShapes?.Dictionary)")]
		IntPtr Constructor (ARBlendShapeLocationOptions blendShapes);

		[Export ("vertexCount")]
		nuint VertexCount { get; }

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("vertices")]
		IntPtr GetRawVertices ();

		[Export ("textureCoordinateCount")]
		nuint TextureCoordinateCount { get; }

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("textureCoordinates")]
		IntPtr GetRawTextureCoordinates ();

		[Export ("triangleCount")]
		nuint TriangleCount { get; }

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("triangleIndices")]
		IntPtr GetRawTriangleIndices ();
	}

	[iOS (11,0)]
	[NoWatch, NoTV, NoMac]
	[BaseType (typeof(SCNGeometry))]
	[DisableDefaultCtor]
	interface ARSCNFaceGeometry {
		[Static]
		[Export ("faceGeometryWithDevice:")]
		[return: NullAllowed]
		ARSCNFaceGeometry CreateFaceGeometry (IMTLDevice device);

		[Static]
		[Export ("faceGeometryWithDevice:fillMesh:")]
		[return: NullAllowed]
		ARSCNFaceGeometry CreateFaceGeometry (IMTLDevice device, bool fillMesh);

		[Export ("updateFromFaceGeometry:")]
		void Update (ARFaceGeometry faceGeometry);
	}

	[iOS (11,0)]
	[NoWatch, NoTV, NoMac]
	[BaseType (typeof(ARLightEstimate))]
	[DisableDefaultCtor]
	interface ARDirectionalLightEstimate {
		[Export ("sphericalHarmonicsCoefficients", ArgumentSemantic.Copy)]
		NSData SphericalHarmonicsCoefficients { get; }

		[Export ("primaryLightDirection")]
		Vector3 PrimaryLightDirection {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
		}

		[Export ("primaryLightIntensity")]
		nfloat PrimaryLightIntensity { get; }
	}
}

#endif // XAMCORE_2_0