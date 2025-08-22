import { getCategoriesSsr } from "@/api/categories";
import AdminTable from "@/app/admin/AdminTable";

export default async function AdminCategoriesPage() {
  const categories = await getCategoriesSsr();

  const columns = [
    { key: "id", label: "Category ID", isLink: true },
    { key: "name", label: "Name" },
  ];

  return (
    <>
      <h1>Management Categories</h1>
      <AdminTable
        data={categories}
        columns={columns}
        baseHref="/admin/categories"
        entityName="Categories"
      />
    </>
  );
}
