import { Card, CardContent, CardMedia, Grid2, Typography } from "@mui/material";

import { getProducts } from "@/api/products";
import QuantityAndBuy from "./QuantityAndBuy";

type Props = {
  params: Promise<{ categoryId: string }>;
};
export default async function ProductsPage({ params }: Props) {
  const { categoryId } = await params;
  const products = await getProducts(Number(categoryId));

  return (
    <Grid2 container spacing={3}>
      {products.map((product) => (
        <Grid2 key={product.id} size={{ xs: 12, sm: 6, md: 4 }}>
          <Card>
            <CardMedia
              component="img"
              height="140"
              image={product.image}
              alt={product.title}
            />
            <CardContent>
              <Typography variant="h5">{product.title}</Typography>
              <Typography variant="body1" color="text.secondary">
                {product.description}
              </Typography>
              <Typography variant="h6" color="primary">
                {product.price} €
              </Typography>

              <QuantityAndBuy productId={product.id} />
            </CardContent>
          </Card>
        </Grid2>
      ))}
    </Grid2>
  );
}
