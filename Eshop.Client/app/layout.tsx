import { AppRouterCacheProvider } from "@mui/material-nextjs/v15-appRouter";
import LinearProgress from "@mui/material/LinearProgress";
import type { Navigation } from "@toolpad/core/AppProvider";
import * as React from "react";

import { getCategories } from "@/api/categories";
import { AuthProvider } from "@/components/providers/AuthProvider";
import QueryProvider from "@/components/providers/QueryProvider";

export default async function Layout(props: { children: React.ReactNode }) {
  const categories = await getCategories();
  const navigation: Navigation = categories.map((x) => {
    return {
      segment: `products/${x.id}`,
      title: x.name,
    };
  });

  return (
    <html lang="en" data-toolpad-color-scheme="dark" suppressHydrationWarning>
      <body>
        <AppRouterCacheProvider options={{ enableCssLayer: true }}>
          <React.Suspense fallback={<LinearProgress />}>
            <QueryProvider>
              <AuthProvider navigation={navigation}>
                {props.children}
              </AuthProvider>
            </QueryProvider>
          </React.Suspense>
        </AppRouterCacheProvider>
      </body>
    </html>
  );
}
