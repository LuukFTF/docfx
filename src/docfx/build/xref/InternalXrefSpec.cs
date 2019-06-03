// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Microsoft.Docs.Build
{
    internal class InternalXrefSpec : IXrefSpec
    {
        public string Uid { get; set; }

        public string Href { get; set; }

        public Document DeclairingFile { get; set; }

        public HashSet<Moniker> Monikers { get; set; } = new HashSet<Moniker>();

        public Dictionary<string, Lazy<JValue>> ExtensionData { get; } = new Dictionary<string, Lazy<JValue>>();

        public string GetXrefPropertyValue(string propertyName)
        {
            if (propertyName is null)
                return null;

            return ExtensionData.TryGetValue(propertyName, out var internalValue) && internalValue.Value.Value is string internalStr ? internalStr : null;
        }

        public string GetName() => GetXrefPropertyValue("name");

        public ExternalXrefSpec ToExternalXrefSpec(Context context, Document file)
        {
            var spec = new ExternalXrefSpec
            {
                Uid = Uid,
                Monikers = Monikers,
                Href = Href,
            };
            foreach (var (key, value) in ExtensionData)
            {
                try
                {
                    spec.ExtensionData[key] = GetXrefPropertyValue(key);
                }
                catch (DocfxException ex)
                {
                    context.ErrorLog.Write(file.FilePath, ex.Error);
                }
            }
            return spec;
        }
    }
}