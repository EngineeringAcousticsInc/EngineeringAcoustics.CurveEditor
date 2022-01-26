#region License
/* FNA - XNA4 Reimplementation for Desktop Platforms
 * Copyright 2009-2020 Ethan Lee and the MonoGame Team
 *
 * Released under the Microsoft Public License.
 * See LICENSE for details.
 */
#endregion

#region Using Statements
using System;
#endregion

namespace Microsoft.Xna.Framework.Content
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class ContentSerializerAttribute : Attribute
	{
		#region Public Properties

		public bool AllowNull
		{
			get;
			set;
		}

		/// <summary>
		/// Returns the overriden XML element name or the default "Item".
		/// </summary>
		public string CollectionItemName
		{
			get
			{
				// Return the default if unset.
				if (string.IsNullOrEmpty(_collectionItemName))
				{
					return "Item";
				}

				return _collectionItemName;
			}
			set => _collectionItemName = value;
		}

		public string ElementName
		{
			get;
			set;
		}

		public bool FlattenContent
		{
			get;
			set;
		}

		/// <summary>
		/// Returns true if the default CollectionItemName value was overridden.
		/// </summary>
		public bool HasCollectionItemName => !string.IsNullOrEmpty(_collectionItemName);

		public bool Optional
		{
			get;
			set;
		}

		public bool SharedResource
		{
			get;
			set;
		}

		#endregion

		#region Private Variables

		private string _collectionItemName;

		#endregion

		#region Public Constructor

		/// <summary>
		/// Creates an instance of the attribute.
		/// </summary>
		public ContentSerializerAttribute()
		{
			AllowNull = true;
		}

		#endregion

		#region Public Clone Method

		public ContentSerializerAttribute Clone()
		{
			var clone = new ContentSerializerAttribute
			{
				AllowNull = AllowNull,
				_collectionItemName = _collectionItemName,
				ElementName = ElementName,
				FlattenContent = FlattenContent,
				Optional = Optional,
				SharedResource = SharedResource
			};
			return clone;
		}

		#endregion
	}
}
