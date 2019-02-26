﻿namespace TrekVRApplication {

    public class ZFBrowserConstants {

        public const string BaseUrl = "localhost:4200";
        //public const string BaseUrl = "localGame://index.html";

        public const string AngularGlobalObjectName = "AngularGlobalVariables";
        public const string UnityGlobalObjectName = "UnityGlobalVariables";
        public const string HttpServiceName = "UnityHttpService";
        public const string SearchServiceName = "UnitySearchService";

        public static readonly string AngularGlobalObjectPath = $"window.{AngularGlobalObjectName}";
        public static readonly string AngularComponentContainerPath = $"{AngularGlobalObjectPath}.componentsMap";
        public static readonly string AngularInjectableContainerPath = $"{AngularGlobalObjectPath}.injectablesMap";

        public static readonly string UnityGlobalObjectPath = $"window.{UnityGlobalObjectName}";

    }

}
