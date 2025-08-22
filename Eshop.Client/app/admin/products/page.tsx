import { getProducts } from "@/api/products";
import AdminTable from "@/app/admin/AdminTable";

export default async function AdminProductsPage() {
  const products = await getProducts();

  const columns = [
    { key: "id", label: "Product ID", isLink: true },
    { key: "title", label: "Name" },
    { key: "price", label: "Price" },
  ];

  return (
    <>
      <h1>Management Products</h1>
      <AdminTable
        data={products}
        columns={columns}
        baseHref="/admin/products"
        entityName="Products"
      />
    </>
  );
}
