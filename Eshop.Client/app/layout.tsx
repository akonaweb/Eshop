import { AppRouterCacheProvider } from "@mui/material-nextjs/v15-appRouter";
import LinearProgress from "@mui/material/LinearProgress";
import type { Navigation } from "@toolpad/core/AppProvider";
import { DashboardLayout } from "@toolpad/core/DashboardLayout";
import { NextAppProvider } from "@toolpad/core/nextjs";
import { PageContainer } from "@toolpad/core/PageContainer";
import * as React from "react";

import { getCategories } from "@/api/categories";
import CartProvider from "@/components/providers/CartProvider";
import ToolbarActions from "@/components/ToolbarActions";
import theme from "../theme";

const BRANDING = {
  title: "Eshop",
};

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
            <NextAppProvider
              navigation={navigation}
              branding={BRANDING}
              theme={theme}
            >
              <CartProvider>
                <DashboardLayout
                  sidebarExpandedWidth={300}
                  slots={{
                    toolbarActions: ToolbarActions,
                  }}
                  disableCollapsibleSidebar
                >
                  <PageContainer title="">{props.children}</PageContainer>
                </DashboardLayout>
              </CartProvider>
            </NextAppProvider>
          </React.Suspense>
        </AppRouterCacheProvider>
      </body>
    </html>
  );
}
