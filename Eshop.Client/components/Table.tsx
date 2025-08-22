import Paper from "@mui/material/Paper";
import MuiTable from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import Link from "next/link";

type Column<T> = {
  key: keyof T | string;
  label: string;
  isLink?: boolean;
};

type TableProps<T> = {
  data: T[];
  columns: Column<T>[];
  baseHref: string;
  entityName: string;
};

export default function Table<T extends { id: string | number }>({
  data,
  columns,
  baseHref,
  entityName,
}: TableProps<T>) {
  return (
    <div>
      <TableContainer component={Paper}>
        <MuiTable>
          <TableHead>
            <TableRow>
              {columns.map((col) => (
                <TableCell key={col.key as string}>{col.label}</TableCell>
              ))}
            </TableRow>
          </TableHead>
          <TableBody>
            {data.map((row) => (
              <TableRow key={row.id}>
                {columns.map((col) => {
                  const value = (row as any)[col.key];
                  return (
                    <TableCell key={col.key as string}>
                      {col.isLink ? (
                        <Link href={`${baseHref}/${row.id}`}>{value}</Link>
                      ) : (
                        value
                      )}
                    </TableCell>
                  );
                })}
              </TableRow>
            ))}
          </TableBody>
        </MuiTable>
      </TableContainer>
      <p>
        Total {entityName}: {data.length}
      </p>
    </div>
  );
}
